using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.API.Models
{
    public class Message
    {
      
        public int Id { get; set; }
        [ForeignKey("senderId")]
        public User Sender { get; set; }

        public string senderId { get; set; }
        [ForeignKey("recipientId")]
        public User Recipient { get; set; }

        public string recipientId { get; set; }
        public string content { get; set; }
        public bool isRead { get; set; }
        public DateTime? dateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool senderDeleted { get; set; }
        public bool recipientDeleted { get; set; }

    }
}