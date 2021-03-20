using System.Collections.Generic;

namespace HackNU.Domain
{
    public class CreateResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}