using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Domain;

namespace HackNU.Services
{
    public interface IEventService
    {
        Task<EventCreateResult> CreateAsync(CreateEventContract eventContract);
        Task<IEnumerable<EventSummary>> FindAsync(string city);
    }
}