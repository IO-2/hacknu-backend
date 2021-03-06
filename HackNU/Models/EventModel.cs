using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackNU.Models
{
    public class EventModel
    {
        public EventModel()
        {
            Tags = new List<TagModel>();
            Attending = new List<UserModel>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public long UnixTime { get; set; } 
        [ForeignKey("UserModel")]
        public string OrganizerEmail { get; set; }
        public virtual ICollection<TagModel> Tags { get; set; }
        public virtual ICollection<UserModel> Attending { get; set; } 
    }
}