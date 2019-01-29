using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.API.Models
{
    public class Like
    {
        public string LikerId { get; set; }
        public string LikeeId { get; set; }
        [ForeignKey("LikerId")]
        public virtual User Liker { get; set; }
        [ForeignKey("LikeeId")]

        public virtual User Likee { get; set; }
    }
}