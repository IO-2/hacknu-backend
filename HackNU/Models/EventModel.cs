using System;

namespace HackNU.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public long UnixTime { get; set; } 
        public UserModel Organizer { get; set; }
    }
}