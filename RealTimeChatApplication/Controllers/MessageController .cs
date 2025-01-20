using ChatSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Model;
using RealTimeChatApplication.Service;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChatApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageStore _messageStore;
        private readonly WebSocketHandler _webSocketHandler;

        public MessageController(WebSocketHandler webSocketHandler, MessageStore messageStore)
        {
            _messageStore = messageStore;
            _webSocketHandler = webSocketHandler;
        }

        [HttpGet("{user1Id}/{user2Id}")]
        public IActionResult GetMessages(string user1Id, string user2Id)
        {
            var messages = _messageStore.GetMessages(user1Id, user2Id);
            if (messages == null || !messages.Any())
            {
                return NotFound("No chat history found.");
            }
            return Ok(messages);
        }

        [HttpPost("send-chat")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            if (message == null || string.IsNullOrEmpty(message.FromUserId) || string.IsNullOrEmpty(message.ToUserId) || string.IsNullOrEmpty(message.MessageContent))
            {
                return BadRequest("Message must contain FromUserId, ToUserId, and MessageContent.");
            }

            _messageStore.AddMessage(message);
 
            var recipientSockets = _webSocketHandler.GetConnections(message.ToUserId);
            if (recipientSockets != null && recipientSockets.Any())
            {
                foreach (var recipientSocket in recipientSockets)
                {
                    await _webSocketHandler.SendMessageToRecipient(recipientSocket, message);
                }
                return Ok(new { Message = "Message sent successfully." });
            }
            else
            {
                return NotFound("Recipient is not online.");
            }
        }

        [HttpGet("conversations/{userId}")]
        public IActionResult GetConversations(string userId)
        {
            var conversations = _messageStore.GetConversations(userId);
            if (conversations == null || !conversations.Any())
            {
                return NotFound("No conversations found.");
            }
            return Ok(conversations);
        }
    }
}
