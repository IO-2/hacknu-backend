using System.Collections;
using System.Collections.Generic;
using HackNU.Models;

namespace HackNU.Contracts.Responses
{
    public class LoadUserResponse
    {
        public string Email { get; set; }
        public string Nickname { get; set; }

        public IEnumerable<EventModel> Events { get; set; }
    }
}