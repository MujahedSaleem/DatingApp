using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]

        public string UserName { get; set; }
        [Required,RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$")]
        public string Password { get; set; }
        [Required]

        public string city { get; set; }
        [Required]

        public string country { get; set; }
        [Required]

        public DateTime dateOfBirth { get; set; }
        [Required]

        public string gender { get; set; }
        [Required]

        public string knownAs { get; set; }
        public string photosUrl { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            this.Created = DateTime.Now;
            this.LastActive = DateTime.Now;

        }


    }
}