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
        Task<IList<EventSummary>> FindAsync(string city);
        Task<IList<EventSummary>> FindSortByDateAsync(string city, bool dateAscending);
        Task<IList<EventSummary>> FindNearestAsync(string city, float longitude, float latitude);
        IList<EventSummary> Query(IList<EventSummary> list, string query);
    }
}