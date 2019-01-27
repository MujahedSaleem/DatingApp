using System;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.Dtos
{
    public class UserForDetailsDto
    {
         public string UserName { get; set; }
        public string gander { get; set; }

        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string lookingFor { get; set; }

        public string intrests { get; set; }
        public string city { get; set; }
        public string Country { get; set; }
        
        public string PhotosUrl { get; set; }
        public ICollection<UserPhotoDto> Photos { get; set; }
    }
}