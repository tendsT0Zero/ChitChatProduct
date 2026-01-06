using ChitChatProduct.API.DTOs;
using ChitChatProduct.API.DTOs.User;
using ChitChatProduct.API.Models;
using ChitChatProduct.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChitChatProduct.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid client request"
                });
            }

            var response = await _userService.CreateUserAsync(createUserDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUserAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid User ID"
                });
            }

            var response = await _userService.GetUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid client request"
                });
            }

            var response = await _userService.UpdateUserAsync(id, updateUserDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid User ID"
                });
            }

            var response = await _userService.DeleteUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}