using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        //A product can have many conversation
        public ICollection<Conversation>? Conversations { get; set; }
    }
}
