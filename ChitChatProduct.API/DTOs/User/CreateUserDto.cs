using ChitChatProduct.API.Models;
using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.DTOs.User
{
    public class CreateUserDto
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
