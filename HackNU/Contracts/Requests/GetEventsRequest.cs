using System.Collections.Generic;

namespace HackNU.Contracts.Requests
{
#nullable enable
    public class GetEventsRequest
    {
        public string City { get; set; }
        public string? Query { get; set; }
        public bool? DateAscending { get; set; }
        public EventLocationRequestBody? EventLocation { get; set; }
        public ICollection<int>? Tags { get; set; }
    }

    public class EventLocationRequestBody
    {
        public bool Ascending { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}