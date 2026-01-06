using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.DTOs.Product
{
    public class ProductResponseDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int UserId { get; set; }
        public string UserName { get; set; }

    }
}
