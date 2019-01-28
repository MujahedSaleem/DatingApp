using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.ApI.Data;
using DatingApp.API.HelpersAndExtentions;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
 
    public class DatingRepository : IDatingRepository
    {
        private readonly IMapper _mapper;

        private readonly ApplicationDbContext _db;
        public DatingRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }
        public void Add<T>(T entity) where T : class
        {
            _db.Entry(entity).State = EntityState.Added;

        }

        public void Delete<T>(T entity) where T : class
        {
            _db.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<Photo> GetMainPhoto(string userID)
        {
            return await _db.Photos.Where(a => a.UserId == userID).FirstOrDefaultAsync(p => p.IsMain);

        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _db.Photos.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<User> GetUser(string Id)
        {
            var user = await _db.User.Include(p => p.Photos).FirstOrDefaultAsync(a => a.Id == Id);
            return user;
        }

        public async Task<bool> SaveAll()
        {

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<PageList<User>> Users(UserParams userParams)
        {
            var users = _db.User.Include(a => a.Photos).Where(a => a.Id != userParams.UserId).AsQueryable();
            if (userParams.Gender != "all")
            {
                users = users.Where(a => a.gander == userParams.Gender);
            }
             if (userParams.name != "all")
            {
                users = users.Where(a => a.UserName.ToLower().Contains(userParams.name.ToLower()));
            }
            if(!string.IsNullOrEmpty(userParams.orderBy)){
               switch (userParams.orderBy)
               {
                   case "created":
                   users=users.OrderByDescending(a=>a.Created);
                   break;
               
                  default:
                   users=users.OrderByDescending(a=>a.LastActive);
                   break;

                   
               }
            }
            if (userParams.minAge != 18 && userParams.maxAge != 99)
            {
                users = users.Where(a => a.DateOfBirth.Age() > userParams.minAge && a.DateOfBirth.Age() < userParams.maxAge);
            }
            else if (userParams.minAge != 18)
            {
                users = users.Where(a => a.DateOfBirth.Age() > userParams.minAge);
            }
            else
            {
                users = users.Where(a => a.DateOfBirth.Age() < userParams.maxAge);
            }
            return await PageList<User>.CreateAsync(users, userParams.Pagenumber, userParams.PageSize);

        }
    }
}