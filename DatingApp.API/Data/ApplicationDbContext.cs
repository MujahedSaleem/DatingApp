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
        protected override void OnModelCreating(ModelBuilder builder){
                    base.OnModelCreating(builder);

            builder.Entity<Like>()
            .HasKey(k=> new {k.LikerId,k.LikeeId});
            builder.Entity<Like>()
            .HasOne(l=>l.Likee)
            .WithMany(l=>l.Likers)
            .HasForeignKey(u=>u.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
            .HasKey(k=> new {k.LikerId,k.LikeeId});
            builder.Entity<Like>()
            .HasOne(l=>l.Liker)
            .WithMany(l=>l.Likees)
            .HasForeignKey(u=>u.LikerId)
            .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Value> Values { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

    }
}
