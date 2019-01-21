using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.ApI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>


    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)

        {

        }
        public DbSet<Value> Values { get; set; }
        public DbSet<User> User { get; set; }

    }
}
