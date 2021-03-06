using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Castle.Core.Internal;
using HackNU.Contracts;
using HackNU.Contracts.Requests;
using HackNU.Contracts.Responses;
using HackNU.Data;
using HackNU.Domain;
using HackNU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HackNU.Services
{
    public class EventService : IEventService
    {
        private readonly DataContext _context;
        private readonly UserManager<UserModel> _userManager;
        
        public EventService(DataContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<CreateResult> CreateAsync(CreateEventContract eventContract, string organizerEmail)
        {
            EventModel newEvent = new EventModel
            {
                Name = eventContract.Name,
                Description = eventContract.Description,
                UnixTime = eventContract.UnixTime,
                City = eventContract.City,
                Longitude = eventContract.Longitude,
                Latitude = eventContract.Latitude,
                OrganizerEmail = organizerEmail
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            
            foreach (var tag in eventContract.TagIds)
            {
                await SubscribeAsync(organizerEmail, newEvent.Id, tag);
            }

            return new CreateResult
            {
                Success = true
            };
        }

        public async Task<IList<EventSummary>> FindAsync(GetEventsRequest request)
        {
            var byCity = FindInCity(request.City);
            var result = byCity;

            // Filter done
            result = result.Where(x => x.UnixTime > DateTimeOffset.Now.ToUnixTimeSeconds()).ToList();

            // Filter by today, tomorrow or anytime
            if (request.DayCount != null)
            {
                var now = DateTimeOffset.Now.AddDays(request.DayCount ?? 0).ToUnixTimeSeconds();
                var end = DateTimeOffset.Now.AddDays(request.DayCount + 1 ?? 1).ToUnixTimeSeconds();

                result = result
                    .Where(x => x.UnixTime > now && x.UnixTime < end)
                    .ToList();
            }

            // Filter by search query
            if (request.Query != null)
            {
                result = Query(result, request.Query);
            }
            
            // Sort by date query
            if (request.DateAscending != null)
            {
                result = SortByDate(result, request.DateAscending ?? true);
            }
            
            // Sort by user location
            if (request.UserLocation != null)
            {
                result = SortByLocation(result, request.UserLocation.Longitude, request.UserLocation.Latitude,
                    request.UserLocation.Ascending);
            }
            
            // Filter by tags
            if (!request.Tags.IsNullOrEmpty())
            {
                result = result.Where(x =>
                    x.Tags.Any(xx =>
                        request.Tags!.Contains(xx.Id)))
                    .ToList();
            }

            return result;
        }

        public async Task<SubscribeResult> SubscribeAsync(string email, int eventId, int tagId)
        {
            var eventToSubscribe = await _context.Events.FirstOrDefaultAsync(x => x.Id == eventId && x.OrganizerEmail == email);
            var tagToPin = await _context.Tags.FirstOrDefaultAsync(x => x.Id == tagId);

            if (eventToSubscribe == null)
            {
                return new SubscribeResult
                {
                    Errors = new[] {"Event does not exists"}
                };
            }

            if (tagToPin == null)
            {
                return new SubscribeResult
                {
                    Errors = new[] {"Tag does not exists"}
                };
            }

            eventToSubscribe.Tags.Add(tagToPin);
            tagToPin.EventsWithTag.Add(eventToSubscribe);
            await _context.SaveChangesAsync();
            
            return new SubscribeResult
            {
                Success = true
            };
        }

        public async Task<ICollection<string>> GetCities()
        {
            var events = _context.Events.ToList();
            var cities = events
                .Select(x => x.City)
                .ToHashSet();
            return cities;
        }

        private IList<EventSummary> SortByDate(IList<EventSummary> list, bool ascending)
        {
            var result = new List<EventSummary>();
            
            if (!ascending)
            {
                result = list.OrderByDescending(x => (x.UnixTime - DateTimeOffset.Now.ToUnixTimeSeconds())).ToList();
            }
            else
            {
                result = list.OrderBy(x => (x.UnixTime - DateTimeOffset.Now.ToUnixTimeSeconds())).ToList();
            }

            return result;
        }

        private IList<EventSummary> SortByLocation(IList<EventSummary> list, float longitude, float latitude, bool ascending)
        {
            var result = new List<EventSummary>();
            if (!ascending)
            {
                result = list.OrderBy(x =>
                {
                    var loc = new Location {Latitude = x.Latitude, Longitude = x.Longitude};
                    var point = new Location {Latitude = latitude, Longitude = longitude};

                    return CalculateDistance(loc, point);
                }).ToList();
            }
            else
            {
                result = list.OrderByDescending(x =>
                {
                    var loc = new Location {Latitude = x.Latitude, Longitude = x.Longitude};
                    var point = new Location {Latitude = latitude, Longitude = longitude};

                    return CalculateDistance(loc, point);
                }).ToList();
            }

            return result.ToList();
        }

        private IList<EventSummary> Query(IList<EventSummary> list, string query)
        {
            var result = list.Where(x => x.Name.ToLower().Contains(query.ToLower()));
            return result.ToList();
        }

        private double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        private IList<EventSummary> FindInCity(string city)
        {
            var events = _context.Events
                .Where(x => x.City == city)
                .ToList();

            var result = new List<EventSummary>();
            
            var tags = new List<TagSummary>();
            
            foreach (var x in events)
            {
                foreach (var t in x.Tags)
                {
                    tags.Add(new TagSummary
                    {
                        Id = t.Id,
                        Text = t.Text
                    });
                }
                result.Add(new EventSummary{
                    Id = x.Id,
                    City = x.City,
                    Description = x.Description,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Name = x.Name,
                    OrganizerEmail = x.OrganizerEmail,
                    Tags = tags,
                    UnixTime = x.UnixTime
                });
                tags = new List<TagSummary>();
            }

            return result;
        }
    }

    public struct Location
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}