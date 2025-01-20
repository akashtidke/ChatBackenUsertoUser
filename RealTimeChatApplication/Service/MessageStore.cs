using RealTimeChatApplication.Model;
using System.Collections.Concurrent;
using System.Linq;

namespace RealTimeChatApplication.Service
{
    public class MessageStore
    {


        private readonly ConcurrentBag<Message> _messages = new ConcurrentBag<Message>();

      
        public void AddMessage(Message message)
        {
            _messages.Add(message);
        }

       
        public IEnumerable<Message> GetMessages(string user1Id, string user2Id)
        {
            return _messages
                .Where(m => (m.FromUserId == user1Id && m.ToUserId == user2Id) ||
                            (m.FromUserId == user2Id && m.ToUserId == user1Id))
                .OrderBy(m => m.Timestamp); 
            //OrderBy
        }


        public IEnumerable<Conversation> GetConversations(string userId)
        {
            var conversations = _messages
                .Where(m => m.FromUserId == userId || m.ToUserId == userId)
                .GroupBy(m => m.FromUserId == userId ? m.ToUserId : m.FromUserId)
                .Select(g => new Conversation
                {
                    UserId = g.Key,
                    LastMessage = g.OrderBy(m => m.Timestamp).Last().MessageContent,
                    Timestamp = g.OrderBy(m => m.Timestamp).Last().Timestamp
                });

            return conversations;
        }

    }
}
