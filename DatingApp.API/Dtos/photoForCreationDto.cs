using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{
    public class photoForCreationDto
    {
      
        public string url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime dateAdded { get; set; }
        public string PublicID { get; set; }
        public photoForCreationDto()
        {
            this.dateAdded = DateTime.Now;
        }
    }
}