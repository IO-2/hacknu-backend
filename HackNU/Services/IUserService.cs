using System.Threading.Tasks;
using HackNU.Contracts.Responses;
using HackNU.Domain;

namespace HackNU.Services
{
    public interface IUserService
    {
        Task<LoadUserResponse> LoadUserAsync(string email);
        Task<SubscribeUserResult> SubscribeAsync(string email, int id);
    }
}