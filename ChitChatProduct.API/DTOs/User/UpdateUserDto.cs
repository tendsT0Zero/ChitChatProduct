using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.DTOs.User
{
    public class UpdateUserDto
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
