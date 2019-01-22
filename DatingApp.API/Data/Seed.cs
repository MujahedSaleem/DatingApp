using System.Collections.Generic;
using DatingApp.ApI.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthRepository repository;
        public Seed(ApplicationDbContext db,IAuthRepository repos)
        {
            repository=repos;
            _db=db;
        }
        public  void SeedUsers(){
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                var password= user.PasswordHash;
                user.PasswordHash=null;
                repository.Register(user,password);
            }
            
        }
    }
}