using Microsoft.AspNetCore.Identity;

namespace HackNU.Models
{
    public class UserModel : IdentityUser
    {
        public string Nickname { get; set; }
        public EventModel Events { get; set; }
    }
}