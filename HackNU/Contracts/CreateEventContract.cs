using System.ComponentModel.DataAnnotations;

namespace HackNU.Contracts
{
    public class CreateEventContract
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        [EmailAddress]
        public string OrganizerEmail { get; set; }
        public long UnixTime { get; set; }
    }
}