using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GatewaySmsAPI.DbOperations
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        public string OtpArea { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime SentTime { get; set; }
    }
}