namespace RealTimeChatApplication.Model
{
    public class Conversation
    {
        public string UserId { get; set; }
        public string LastMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
