using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Product;

namespace ChitChatProduct.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<APIResponse> CreateProductAsync(CreateProductDto product);
        Task<APIResponse> GetAllProductsAsync();
        Task<APIResponse> GetProductByIdAsync(int id);
        Task<APIResponse> UpdateProductAsync(int id, UpdateProductDto product);
        Task<APIResponse> DeleteProductAsync(int id);
    }
}
