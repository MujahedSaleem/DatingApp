using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.ApI.Data;
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

        public async Task<User> GetUser(string Id)
        {
            var user = await _db.User.Include(p => p.Photos).FirstOrDefaultAsync(a => a.Id == Id);
            return user;
        }

        public async Task<bool> SaveAll()
        {

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> Users()
        {
            return await _db.User.Include(a => a.Photos).ToListAsync();
        }
    }
}