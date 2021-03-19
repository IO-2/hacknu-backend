using System.Linq;
using System.Threading.Tasks;
using HackNU.Contracts.Responses;
using HackNU.Data;
using HackNU.Domain;
using HackNU.Models;
using Microsoft.AspNetCore.Identity;

namespace HackNU.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly DataContext _context;
        
        public UserService(UserManager<UserModel> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        public async Task<LoadUserResponse> LoadUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }
            
            var loadUser = new LoadUserResponse
            {
                Nickname = user.Nickname,
                Email = user.Email,
                Events = user.Events
            };

            return loadUser;
        }

        public async Task<SubscribeUserResult> SubscribeAsync(string email, int id)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new SubscribeUserResult
                {
                    Errors = new []{"User with specified email not found"}
                };
            }
            
            // Why this line adds event to user???????????????? WHYYYYY
            var toAttend = _context.Events.FirstOrDefault(x => x.Id == id);

            if (toAttend == null)
            {
                return new SubscribeUserResult
                {
                    Errors = new[]{$"Event with id {id} does not exists"}
                };
            }

            user.Events.Add(toAttend);

            // Not updating properly
            var identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                return new SubscribeUserResult
                {
                    Errors = identityResult.Errors.Select(x => x.Description)
                };
            }
            
            return new SubscribeUserResult
            {
                Success = true
            };
        }
    }
}