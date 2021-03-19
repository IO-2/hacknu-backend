using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Data;
using HackNU.Domain;
using HackNU.Models;
using Microsoft.AspNetCore.Identity;

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
        
        public async Task<EventCreateResult> Create(CreateEventContract eventContract)
        {
            UserModel organizer = await _userManager.FindByEmailAsync(eventContract.OrganizerEmail);

            EventModel newEvent = new EventModel
            {
                Name = eventContract.Name,
                Description = eventContract.Description,
                UnixTime = eventContract.UnixTime,
                City = eventContract.City,
                Longitude = eventContract.Longitude,
                Latitude = eventContract.Latitude,
                Organizer = organizer
            };
            
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            return new EventCreateResult
            {
                Success = true
            };
        }
    }
}