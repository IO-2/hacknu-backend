using System.Collections.Generic;

namespace HackNU.Domain
{
    public class SubscribeUserResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}