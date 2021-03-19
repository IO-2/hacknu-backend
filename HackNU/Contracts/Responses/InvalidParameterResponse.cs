using System.Collections.Generic;

namespace HackNU.Contracts.Responses
{
    public class InvalidParameterResponse
    {
        public IEnumerable<string> Errors { get; set; } = new[] {"Invalid arguments"};
    }
}