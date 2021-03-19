using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Domain;

namespace HackNU.Services
{
    public interface IEventService
    {
        Task<EventCreateResult> Create(CreateEventContract eventContract);
    }
}