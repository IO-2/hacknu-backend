using System.Collections.Generic;

namespace HackNU.Models
{
    public class TagModel
    {
        public TagModel()
        {
            EventsWithTag = new List<EventModel>();
        }
        
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ICollection<EventModel> EventsWithTag { get; set; }
    }
}