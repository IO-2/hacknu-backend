using System.Collections.Generic;
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
        public long UnixTime { get; set; }
        public ICollection<int> TagIds { get; set; }
    }
}