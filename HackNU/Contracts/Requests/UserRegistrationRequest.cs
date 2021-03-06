using System.ComponentModel.DataAnnotations;

namespace HackNU.Contracts.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
    }
}