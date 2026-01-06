using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Message;

namespace ChitChatProduct.API.Services.Interfaces
{
    public interface IMessageService
    {
        Task<APIResponse> SendMessageAsync(SendMessageDto request);
        Task<APIResponse> GetMessagesByConversationAsync(int conversationId);

        Task<APIResponse> MarkMessagesAsReadAsync(int conversationId, int readerId);
        Task<APIResponse> GetUnreadCountAsync(int userId);
    }
}