namespace ChitChatProduct.API.DTOs
{
    public class APIResponse
    {
        public object ResponseObject { get; set; } = null;
        public bool IsSuccess { get; set; }=false;
        public string Message { get; set;  }=string.Empty;

    }
}
