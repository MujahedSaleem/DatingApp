using System;

namespace DatingApp.API.Dtos
{
    public class MessageForReturnDto
    {        public int Id { get; set; }

        public string senderId { get; set; }
        public  string senderKnownAs { get; set; }
        public string senderPhotoUrl { get; set; }
        public string recipientId { get; set; }
        public  string recipientKnwonAs { get; set; }
        public string recipientPhotoUrl { get; set; }
        public string content { get; set; }
        public bool isRead { get; set; }
        public DateTime? dateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool senderDeleted { get; set; }
        public bool recipientDeleted { get; set; }
    }
}