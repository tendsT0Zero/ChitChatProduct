using ChitChatProduct.API.Models;
using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.DTOs.Product
{
    public class CreateProductDto
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
