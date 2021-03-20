using System.Collections.Generic;

namespace HackNU.Contracts.Requests
{
#nullable enable
    public class GetEventsRequest
    {
        public string City { get; set; }
        public string? Query { get; set; }
        public bool? DateAscending { get; set; }
        public UserLocationRequestBody? UserLocation { get; set; }
        public ICollection<int>? Tags { get; set; }
        public int? DayCount { get; set; }
    }

    public class UserLocationRequestBody
    {
        public bool Ascending { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}