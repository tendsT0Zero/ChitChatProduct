using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        // each conversation based on a product. one conversation related to one product
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //lets make seller<->buyer only one conversation thread
        [Required]
        public int SellerId { get; set; }
        [Required]
        public int BuyerId { get; set; }

        public User Seller {  get; set; }
        public User Buyer { get; set; }
        //a conversation has many Messages
        public ICollection<ChatMessage> Messages { get; set; }
    }
}
