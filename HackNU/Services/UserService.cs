using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Contracts.Responses;
using HackNU.Data;
using HackNU.Domain;
using HackNU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace HackNU.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        
        public UserService(DataContext context)
        {
            _context = context;
        }
        
        public async Task<LoadUserResponse> LoadUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            var eventSummary = new List<EventSummary>();
            
            foreach (var e in user.Events)
            {
                var tags = new List<TagSummary>();
                foreach (var tag in e.Tags)
                {
                    tags.Add(new TagSummary
                    {
                        Id = tag.Id,
                        Text = tag.Text
                    });
                }
                eventSummary.Add(new EventSummary
                {
                    Id = e.Id,
                    City = e.City,
                    Description = e.Description,
                    Latitude = e.Latitude,
                    Longitude = e.Longitude,
                    Name = e.Name,
                    OrganizerEmail = e.OrganizerEmail,
                    Tags = tags,
                    UnixTime = e.UnixTime
                });
            }

            var loadUser = new LoadUserResponse
            {
                Nickname = user.Nickname,
                Email = user.Email,
                Events = eventSummary
            };

            return loadUser;
        }

        public async Task<SubscribeResult> SubscribeAsync(string email, int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new SubscribeResult
                {
                    Errors = new []{"User with specified email not found"}
                };
            }
            
            var toAttend = _context.Events.FirstOrDefault(x => x.Id == id);

            if (toAttend == null)
            {
                return new SubscribeResult
                {
                    Errors = new[]{$"Event with id {id} does not exists"}
                };
            }

            if (user.Events.FirstOrDefault(x => x.Id == id) != null)
            {
                return new SubscribeResult
                {
                    Errors = new[]{"User is already subscribed to event"}
                };
            }
            user.Events.Add(toAttend);

            await _context.SaveChangesAsync();

            // if (!identityResult.Succeeded)
            // {
            //     return new SubscribeUserResult
            //     {
            //         Errors = identityResult.Errors.Select(x => x.Description)
            //     };
            // }
            
            return new SubscribeResult
            {
                Success = true
            };
        }
    }
}