using System.Collections.Generic;

namespace HackNU.Domain
{
    public class EventCreateResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}