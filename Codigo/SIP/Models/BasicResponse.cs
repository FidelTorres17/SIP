namespace SIP.Models
{
    public class BasicResponse
    {
        public int ReturnCode { get; set; }
        public int? ReturnId { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
    }
}
