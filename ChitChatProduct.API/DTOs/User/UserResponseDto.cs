using ChitChatProduct.API.Models;
using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.DTOs.User
{
    public class UserResponseDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public ICollection<string>? ProductIds { get; set; }
    }
}
