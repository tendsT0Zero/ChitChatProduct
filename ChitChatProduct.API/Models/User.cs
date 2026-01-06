using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        // Navigation Properties
        public ICollection<Product>? Products { get; set; } 

        // Chats where this user is the Seller (derived from products they own)
        public ICollection<Conversation>? SalesConversations { get; set; }

        // Chats where this user is the Buyer (initiated by them)
        public ICollection<Conversation>? BuyingConversations { get; set; }

        public ICollection<ChatMessage>? SentMessages { get; set; }
        public ICollection<ChatMessage>? ReceivedMessages { get; set; }
    }
}
