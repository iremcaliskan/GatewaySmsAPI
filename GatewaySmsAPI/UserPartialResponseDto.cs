using System;

namespace GatewaySmsAPI
{
    public class UserPartialResponseDto
    {
        public int MessageId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime SentTime { get; set; }
    }
}