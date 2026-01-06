using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.DTOs.Product
{
    public class UpdateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
