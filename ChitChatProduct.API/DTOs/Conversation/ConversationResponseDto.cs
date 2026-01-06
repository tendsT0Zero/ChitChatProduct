namespace ChitChatProduct.API.DTOs.Conversation
{
    public class ConversationResponseDto
    {
        public int Id { get; set; }
        public DateTime LastMessageAt { get; set; }

        // Context info
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        // Who am I talking to?
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; }
    }
}
