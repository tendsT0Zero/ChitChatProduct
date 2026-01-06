using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.User;

namespace ChitChatProduct.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<APIResponse> CreateUserAsync(CreateUserDto user);
        Task<APIResponse> UpdateUserAsync(int userId,UpdateUserDto user);
        Task<APIResponse> DeleteUserAsync(int id);
        Task<APIResponse> GetAllUserAsync();
        Task<APIResponse> GetUserAsync(int id);

    }
}
