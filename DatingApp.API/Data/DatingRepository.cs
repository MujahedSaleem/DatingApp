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

        public async Task<Like> GetLike(string userId, string recipientId)
        {
            return await _db.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);

        }

        public async Task<Photo> GetMainPhoto(string userID)
        {
            return await _db.Photos.Where(a => a.UserId == userID).FirstOrDefaultAsync(p => p.IsMain);

        }

        public async Task<Message> GetMessage(int id)
        {
            return await _db.Messages.FirstOrDefaultAsync(a => a.Id == id);

        }

        public async Task<PageList<Message>> GetMessages(MessageParams messageParams)
        {
            var messages = _db.Messages.Include(u => u.Sender)
            .ThenInclude(u => u.Photos)
            .Include(u => u.Recipient)
            .ThenInclude(u => u.Photos).AsQueryable();
            
            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages. Where(u => u.recipientId == messageParams.UserId && u.recipientDeleted==false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.senderId == messageParams.UserId && u.senderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.recipientId == messageParams.UserId && u.isRead == false  &&u.recipientDeleted == false);
                    break;

            }

            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PageList<Message>
            .CreateAsync(messages, messageParams.Pagenumber, messageParams.PageSize);
        }
        public async Task<IEnumerable<Message>> GetMessageThread(string userId, string recipientId)
        {
            var messages = await _db.Messages.Include(u => u.Sender)
                       .ThenInclude(u => u.Photos)
                       .Include(u => u.Recipient)
                       .ThenInclude(u => u.Photos)
                       .Where(u => u.recipientId == userId && u.recipientDeleted == false && u.senderId == recipientId 
                       || u.recipientId == recipientId && u.senderDeleted == false && u.senderId == userId)
                       .OrderBy(a => a.MessageSent).ToListAsync();
            var m =  messages.AsQueryable().TakeLast(10).ToList();

            return m;
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
            if (!string.IsNullOrEmpty(userParams.Gender))
            {
                users = users.Where(a => a.gander == userParams.Gender);
            }
            if (userParams.name != "all")
            {
                users = users.Where(a => a.UserName.ToLower().Contains(userParams.name.ToLower()));
            }
            if (!string.IsNullOrEmpty(userParams.orderBy))
            {
                switch (userParams.orderBy)
                {
                    case "created":
                        users = users.OrderByDescending(a => a.Created);
                        break;

                    default:
                        users = users.OrderByDescending(a => a.LastActive);
                        break;


                }
            }
            if (userParams.likee)
            {
                var userlike = await getLikeUser(userParams.UserId, userParams.liker);
                users = users.Where(a => userlike.Contains(a.Id));
            }
            else if (userParams.liker)
            {
                var userlikees = await getLikeUser(userParams.UserId, userParams.liker);
                users = users.Where(a => userlikees.Contains(a.Id));

            }
            if (userParams.minAge != 18 && userParams.maxAge != 99)
            {
                users = users.Where(a => a.DateOfBirth.Age() > userParams.minAge && a.DateOfBirth.Age() < userParams.maxAge);
            }
            else if (userParams.minAge != 18)
            {
                users = users.Where(a => a.DateOfBirth.Age() > userParams.minAge);
            }
            else if (userParams.maxAge != 99)
            {
                users = users.Where(a => a.DateOfBirth.Age() < userParams.maxAge);
            }
            return await PageList<User>.CreateAsync(users, userParams.Pagenumber, userParams.PageSize);

        }
        private async Task<IEnumerable<string>> getLikeUser(string userId, bool liker)
        {
            var loggedInUser = await _db.User.Include(a => a.Likees).Include(a => a.Likers).FirstOrDefaultAsync(a => a.Id == userId);

            if (liker)
            {
                return loggedInUser.Likers.Where(a => a.LikeeId == userId).Select(a => a.LikerId);
            }
            else
            {
                return loggedInUser.Likees.Where(a => a.LikerId == userId).Select(a => a.LikeeId);

            }
        }
    }
}