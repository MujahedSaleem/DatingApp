using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.ApI.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public Dictionary<string, string> Names { get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthRepository(ApplicationDbContext db, UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            var Users = _db.User.ToList();
            Names = new Dictionary<string, string>();
            
            foreach (var item in Users)
            {
                Names.Add(item.Id, item.Name);
            }
        }
        public async Task<User> Login(string userName, string Password)
        {
            if (userExistsAsync(userName))
            {
                var result = await _signInManager.PasswordSignInAsync(userName, Password, false, false);
                var users = new User {UserName=userName, Name = userName, Email = userName };
                var results = await _userManager.CreateAsync(users, Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(users,false);
                    
                }
                return users;
            }
            return null;
        }

        public async Task<User> Register(User user, string Password)
        {
            var users = new User { UserName=user.Name, Name = user.Name, Email = user.Name };
            var result = await _userManager.CreateAsync(users, Password);
            if (result.Succeeded)
            {
                return user;
            }
            StringBuilder error = new StringBuilder();
            foreach( var item in result.Errors){
                error.Append(item.Description);
            }
            throw new System.Exception(error.ToString());



        }


        public bool userExistsAsync(string userName)
        {
            if(Names is null)
            return false;
            var name = (Names.FirstOrDefault(a => a.Value == userName)).Value;
            if (name is null)
            {
                return false;
            }
            return true;
        }
    }
}