using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using HackNU.Contracts;
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
        
        public async Task<EventCreateResult> CreateAsync(CreateEventContract eventContract)
        {
            UserModel organizer = await _userManager.FindByEmailAsync(eventContract.OrganizerEmail);

            if (organizer == null)
            {
                return new EventCreateResult
                {
                    Errors = new []{"User with specified email does not exists"}
                };
            }

            EventModel newEvent = new EventModel
            {
                Name = eventContract.Name,
                Description = eventContract.Description,
                UnixTime = eventContract.UnixTime,
                City = eventContract.City,
                Longitude = eventContract.Longitude,
                Latitude = eventContract.Latitude,
                OrganizerEmail = organizer.Email
            };
            
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            return new EventCreateResult
            {
                Success = true
            };
        }

        public async Task<IEnumerable<EventSummary>> FindAsync(string city)
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
            }

            return result;
        }
    }
}