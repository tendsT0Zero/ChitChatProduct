using ChitChatProduct.API.Data;
using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Conversation;
using ChitChatProduct.API.Models;
using ChitChatProduct.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChitChatProduct.API.Services.Implementations
{
    public class ConversationService : IConversationService
    {
        private readonly AppDbContext _context;

        public ConversationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse> StartConversationAsync(StartConversationDto request)
        {
            try
            {
                // Check if Product exists to get the SellerId
                var product = await _context.Products.FindAsync(request.ProductId);
                if (product == null)
                {
                    return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status404NotFound, Message = "Product not found" };
                }

                if (product.UserId == request.BuyerId)
                {
                    return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest, Message = "You cannot start a chat for your own product." };
                }

                // Check if conversation already exists (Idempotency)
                // We use the Unique Index {ProductId, BuyerId} logic here
                var existingConv = await _context.Conversations
                    .FirstOrDefaultAsync(c => c.ProductId == request.ProductId && c.BuyerId == request.BuyerId);

                if (existingConv != null)
                {
                    // Return the existing ID so the frontend can just open that window
                    return new APIResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Conversation already exists",
                        ResponseObject = existingConv.Id
                    };
                }

                // Create new Conversation
                var newConv = new Conversation
                {
                    ProductId = request.ProductId,
                    BuyerId = request.BuyerId,
                    SellerId = product.UserId, 
                    CreatedAt = DateTime.UtcNow,
                    LastMessageAt = DateTime.UtcNow
                };

                await _context.Conversations.AddAsync(newConv);
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Conversation started",
                    ResponseObject = newConv.Id
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { IsSuccess = false, StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message };
            }
        }

        public async Task<APIResponse> GetUserConversationsAsync(int userId)
        {
            try
            {
                // Get all chats where user is Buyer OR Seller
                var conversations = await _context.Conversations
                    .Include(c => c.Product)
                    .Include(c => c.Buyer)
                    .Include(c => c.Seller)
                    .Where(c => c.BuyerId == userId || c.SellerId == userId)
                    .OrderByDescending(c => c.LastMessageAt) // Show newest first
                    .ToListAsync();

                if (!conversations.Any())
                {
                    return new APIResponse { IsSuccess = true, StatusCode = StatusCodes.Status204NoContent, Message = "No conversations found" };
                }

                // Map to DTO
                var responseList = conversations.Select(c => new ConversationResponseDto
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    LastMessageAt = c.LastMessageAt ?? c.CreatedAt,
                    // Dynamic logic: If I am the Buyer, show me the Seller's name. If I am Seller, show Buyer's name.
                    OtherUserId = (c.BuyerId == userId) ? c.SellerId : c.BuyerId,
                    OtherUserName = (c.BuyerId == userId) ? c.Seller.Name : c.Buyer.Name
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
    }
}