namespace ChitChatProduct.API.DTOs.Message
{
    public class SendMessageDto
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
    }
}
