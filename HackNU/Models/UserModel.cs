using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace HackNU.Models
{
    public class UserModel : IdentityUser
    {
        public UserModel()
        {
            Events = new List<EventModel>();
        }
        
        public string Nickname { get; set; }
        public virtual ICollection<EventModel> Events { get; set; }
    }
}