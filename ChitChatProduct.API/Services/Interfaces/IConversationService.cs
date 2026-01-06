using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Conversation;

namespace ChitChatProduct.API.Services.Interfaces
{
    public interface IConversationService
    {
        Task<APIResponse> StartConversationAsync(StartConversationDto request);
        Task<APIResponse> GetUserConversationsAsync(int userId);
    }
}
