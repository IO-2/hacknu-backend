namespace HackNU.Contracts.Requests
{
    public class UserSubscribeToEventRequest
    {
        public string Email { get; set; }
        public int EventId { get; set; }
    }
}