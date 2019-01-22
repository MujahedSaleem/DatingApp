using System;

namespace DatingApp.API.Dtos
{
    public class UserForListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string gander { get; set; }

        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
       
        public string city { get; set; }
        public string Country { get; set; }
        
        public string PhotosUrl { get; set; } 
    }
}