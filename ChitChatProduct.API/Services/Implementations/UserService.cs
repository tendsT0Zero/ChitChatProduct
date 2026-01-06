using ChitChatProduct.API.Data;
using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.User;
using ChitChatProduct.API.Models;
using ChitChatProduct.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChitChatProduct.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<APIResponse> CreateUserAsync(CreateUserDto user)
        {
            try
            {
                if(user== null)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode=StatusCodes.Status400BadRequest,
                        Message="Bad Request , Body is empty"
                    };
                    
                }
                var userDomain = new User
                {
                    Name = user.Name,
                    Email = user.Email
                    
                };
                await _context.Users.AddAsync(userDomain);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Successfully created"
                };

            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    StatusCode= StatusCodes.Status500InternalServerError,
                    Message=ex.Message
                };
            }
        }

        public async Task<APIResponse> DeleteUserAsync(int id)
        {
            try
            {
                var existingUser=await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (existingUser == null)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "User not found."
                    };
                }
                _context.Users.Remove(existingUser);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Successfully Deleted"
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

        public async Task<APIResponse> GetAllUserAsync()
        {
            try
            {
                var users = await _context.Users.Include(u=>u.Products).ToListAsync();
                if (users.Any())
                {
                    var userResponses=users.Select(u=>new UserResponseDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        ProductIds=u.Products.Select(p => p.Id).ToList()

                    }).ToList();

                    return new APIResponse
                    {
                        ResponseObject=userResponses,
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Successfully retrived users."
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status204NoContent,
                        Message = "No user found."
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

        public async Task<APIResponse> GetUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Products).FirstOrDefaultAsync(u => u.Id==id);
                if (user!=null)
                {
                    var userResponse =new UserResponseDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        ProductIds = user.Products.Select(p => p.Id).ToList()

                    };

                    return new APIResponse
                    {
                        ResponseObject=userResponse,
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Successfully retrived users."
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status204NoContent,
                        Message = "No user found."
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

        public async Task<APIResponse> UpdateUserAsync(int userId,UpdateUserDto user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (existingUser == null)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "User not found."
                    };
                }
                existingUser.Name= user.Name;
                existingUser.Email= user.Email;
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Successfully updated user."
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
