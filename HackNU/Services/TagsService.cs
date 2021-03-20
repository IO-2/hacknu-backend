using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using HackNU.Contracts;
using HackNU.Data;
using HackNU.Domain;
using HackNU.Models;

namespace HackNU.Services
{
    public class TagsService : ITagsService
    {
        private readonly DataContext _context;
        
        public TagsService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<TagSummary> GetTags()
        {
            var tags = _context.Tags.ToList();
            var result = new List<TagSummary>();

            foreach (var tag in tags)
            {
                result.Add(new TagSummary
                {
                    Id = tag.Id,
                    Text = tag.Text
                });
            }

            return result;
        }

        public async Task<CreateResult> Create(string text)
        {
            var find = _context.Tags.Where(x => x.Text == text).ToList();

            if (!find.IsNullOrEmpty())
            {
                return new CreateResult
                {
                    Errors = new []{"Tag is already exists"}
                };
            }

            _context.Tags.Add(new TagModel
            {
                Text = text
            });
            await _context.SaveChangesAsync();
            
            return new CreateResult
            {
                Success = true
            };
        }
    }
}