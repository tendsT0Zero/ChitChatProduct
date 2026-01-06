namespace ChitChatProduct.API.DTOs.Message
{
    public class MessageResponseDto
    {
        public long Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
