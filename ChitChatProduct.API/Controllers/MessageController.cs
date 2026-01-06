using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Message;
using ChitChatProduct.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChitChatProduct.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto request)
        {
            if (request == null) return BadRequest();
            var response = await _messageService.SendMessageAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{conversationId}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHistory(int conversationId)
        {
            var response = await _messageService.GetMessagesByConversationAsync(conversationId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("mark-read/{conversationId}/{readerId}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAsRead(int conversationId, int readerId)
        {
            // In a real app, 'readerId' would come from the logged-in User Token, not the URL
            var response = await _messageService.MarkMessagesAsReadAsync(conversationId, readerId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("unread-count/{userId}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnreadCount(int userId)
        {
            var response = await _messageService.GetUnreadCountAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}