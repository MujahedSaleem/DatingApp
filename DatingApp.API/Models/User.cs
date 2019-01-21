using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Models
{
    public class User :IdentityUser
    {
        public string Name { get; set; }

    }
}