using ChitChatProduct.API.Data;
using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.Product;
using ChitChatProduct.API.Models;
using ChitChatProduct.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChitChatProduct.API.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse> CreateProductAsync(CreateProductDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Bad Request, Body is empty"
                    };
                }

                
                var ownerExists = await _context.Users.AnyAsync(u => u.Id == productDto.UserId);
                if (!ownerExists)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "User (Owner) not found."
                    };
                }

                var productDomain = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    UserId = productDto.UserId
                };

                await _context.Products.AddAsync(productDomain);
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Product successfully created"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse> GetAllProductsAsync()
        {
            try
            {
                
                var products = await _context.Products.Include(p => p.User).ToListAsync();

                if (products.Any())
                {
                    var productResponses = products.Select(p => new ProductResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        UserName = p.User != null ? p.User.Name : "Unknown",
                        UserId = p.UserId
                    }).ToList();

                    return new APIResponse
                    {
                        ResponseObject = productResponses,
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Successfully retrieved products."
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status204NoContent,
                        Message = "No products found."
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    var productResponse = new ProductResponseDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        UserName = product.User != null ? product.User.Name : "Unknown",
                        UserId = product.UserId
                    };

                    return new APIResponse
                    {
                        ResponseObject = productResponse,
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Successfully retrieved product."
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Product not found."
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse> UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            try
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (existingProduct == null)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Product not found."
                    };
                }

                existingProduct.Name = productDto.Name;
                existingProduct.Price = productDto.Price;

                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Successfully updated product."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse> DeleteProductAsync(int id)
        {
            try
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (existingProduct == null)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Product not found."
                    };
                }

                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Successfully deleted product."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                };
            }
        }
    }
}