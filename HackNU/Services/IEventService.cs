using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Contracts.Requests;
using HackNU.Domain;

namespace HackNU.Services
{
    public interface IEventService
    {
        Task<CreateResult> CreateAsync(CreateEventContract eventContract);
        Task<IList<EventSummary>> FindAsync(GetEventsRequest request);
    }
}