using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.HelpersAndExtentions;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PageList<User>> Users (UserParams userParams);
        Task<User> GetUser(string Id);
        Task<Photo> GetPhoto(int id);
        
        Task<Photo> GetMainPhoto(string userID);

        Task<Like> GetLike(string userId,string recipientId);

        Task<Message> GetMessage(int id);
        Task<PageList<Message>> GetMessages(MessageParams MessageParams);
        Task<IEnumerable<Message>> GetMessageThread(string userId, string recipientId);
    }
}