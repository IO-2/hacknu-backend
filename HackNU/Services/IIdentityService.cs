using System.Threading.Tasks;
using HackNU.Domain;
using Microsoft.AspNetCore.Authentication;

namespace HackNU.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string nickname, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}