using System.ComponentModel.DataAnnotations;

namespace ChitChatProduct.API.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        // each Message related to one conversation
        [Required]
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        //each message's has one sender and one receiver
        [Required]
        public int SenderId { get; set; }
        public User Sender { get; set; }
        [Required]
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public bool IsRead { get; set; }

    }
}
