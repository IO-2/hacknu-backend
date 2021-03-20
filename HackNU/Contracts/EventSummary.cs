using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HackNU.Models;

namespace HackNU.Contracts
{
    public class EventSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public long UnixTime { get; set; } 
        [ForeignKey("UserModel")]
        public string OrganizerEmail { get; set; }
        public virtual ICollection<TagSummary> Tags { get; set; }
    }
}