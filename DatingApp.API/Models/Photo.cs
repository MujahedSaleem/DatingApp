using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User user { get; set; }

    }
}