using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Conversation;
using ChitChatProduct.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChitChatProduct.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("start")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)] // If exists
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)] // If new
        public async Task<IActionResult> StartConversation([FromBody] StartConversationDto request)
        {
            if (request == null) return BadRequest();
            var response = await _conversationService.StartConversationAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserInbox(int userId)
        {
            var response = await _conversationService.GetUserConversationsAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}