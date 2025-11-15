using BLL.ModelVM.Chat;
using BLL.Services;
using BLL.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("conversations/{userId}")]
        public async Task<ActionResult<List<ConversationVM>>> GetUserConversations(Guid userId)
        {
            try
            {
                var conversations = await _chatService.GetUserConversationsAsync(userId);
                return Ok(conversations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("conversation/{currentUserId}/{otherUserId}")]
        public async Task<ActionResult<List<MessageVM>>> GetConversation(Guid currentUserId, Guid otherUserId)
        {
            try
            {
                var messages = await _chatService.GetConversationAsync(currentUserId, otherUserId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("mark-read/{currentUserId}/{otherUserId}")]
        public async Task<IActionResult> MarkAsRead(Guid currentUserId, Guid otherUserId)
        {
            try
            {
                await _chatService.MarkMessagesAsReadAsync(currentUserId, otherUserId);
                return Ok(new { message = "Messages marked as read" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("send-message/{userId}")]
        public async Task<ActionResult<MessageVM>> SendMessage([FromRoute] Guid userId, [FromBody] CreateMessageVM messageVm)
        {
            try
            {
                var message = await _chatService.SendMessageAsync(userId, messageVm);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("unread-count/{userId}")]
        public async Task<ActionResult<int>> GetUnreadCount(Guid userId)
        {
            try
            {
                var count = await _chatService.GetUnreadCountAsync(userId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<UserVM>> GetUser(Guid userId)
        {
            try
            {
                var user = await _chatService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound(new { error = "User not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
