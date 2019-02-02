using System;

namespace DatingApp.API.Dtos
{
    public class MessageForCreationDto
    {

        public string senderId { get; set; }
        public string recipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public string senderPhotoUrl { get; set; }

        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}