using ChitChatProduct.API.Data;
using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Message;
using ChitChatProduct.API.Models;
using ChitChatProduct.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChitChatProduct.API.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;

        public MessageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse> SendMessageAsync(SendMessageDto request)
        {
            try
            {
                //Validate Conversation
                var conversation = await _context.Conversations
                    .Include(c => c.Buyer)
                    .Include(c => c.Seller)
                    .FirstOrDefaultAsync(c => c.Id == request.ConversationId);

                if (conversation == null)
                {
                    return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status404NotFound, Message = "Conversation not found" };
                }

                // Determine Receiver (The person NOT sending the message)
                int receiverId;
                if (request.SenderId == conversation.BuyerId) receiverId = conversation.SellerId;
                else if (request.SenderId == conversation.SellerId) receiverId = conversation.BuyerId;
                else return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status403Forbidden, Message = "Sender is not part of this conversation" };

                // Create Message
                var newMessage = new ChatMessage
                {
                    ConversationId = request.ConversationId,
                    SenderId = request.SenderId,
                    Content = request.Content,
                    SentAt = DateTime.UtcNow,
                    ReceiverId = receiverId
                };

                await _context.ChatMessages.AddAsync(newMessage);

                // Update Conversation Timestamp 
                conversation.LastMessageAt = newMessage.SentAt;

                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Message sent successfully",
                    ResponseObject = new MessageResponseDto
                    {
                        Id = newMessage.Id,
                        SenderId = newMessage.SenderId,
                        Content = newMessage.Content,
                        SentAt = newMessage.SentAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message };
            }
        }

        public async Task<APIResponse> GetMessagesByConversationAsync(int conversationId)
        {
            try
            {
                var messages = await _context.ChatMessages
                    .Include(m => m.Sender)
                    .Where(m => m.ConversationId == conversationId)
                    .OrderBy(m => m.SentAt) // Oldest first
                    .ToListAsync();

                if (!messages.Any())
                {
                    return new APIResponse { IsSuccess = true, StatusCode = StatusCodes.Status204NoContent, Message = "No messages found" };
                }

                var responseList = messages.Select(m => new MessageResponseDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.Name,
                    Content = m.Content,
                    SentAt = m.SentAt
                }).ToList();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseObject = responseList
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message };
            }
        }

        public async Task<APIResponse> MarkMessagesAsReadAsync(int conversationId, int readerId)
        {
            try
            {
                // Find all unread messages in this conversation sent TO the reader
                var unreadMessages = await _context.ChatMessages
                    .Where(m => m.ConversationId == conversationId
                                && m.ReceiverId == readerId
                                && !m.IsRead)
                    .ToListAsync();

                if (!unreadMessages.Any())
                {
                    return new APIResponse { IsSuccess = true, StatusCode = StatusCodes.Status200OK, Message = "No new messages to mark." };
                }

                // Batch update
                foreach (var msg in unreadMessages)
                {
                    msg.IsRead = true;
                }

                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"{unreadMessages.Count} messages marked as read."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message };
            }
        }

        public async Task<APIResponse> GetUnreadCountAsync(int userId)
        {
            try
            {
                // Efficient Count Query
                var count = await _context.ChatMessages
                    .CountAsync(m => m.ReceiverId == userId && !m.IsRead);

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseObject = new { UnreadCount = count }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message };
            }
        }
    }
}