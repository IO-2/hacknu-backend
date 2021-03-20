using System.Collections.Generic;
using System.Threading.Tasks;
using HackNU.Contracts;
using HackNU.Domain;

namespace HackNU.Services
{
    public interface ITagsService
    {
        IEnumerable<TagSummary> GetTags();
        Task<CreateResult> Create(string text);
    }
}