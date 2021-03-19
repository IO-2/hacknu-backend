using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace HackNU.Models
{
    public class UserModel : IdentityUser
    {
        public string Nickname { get; set; }
        public ICollection<EventModel> Events { get; set; } = new Collection<EventModel>();
    }
}