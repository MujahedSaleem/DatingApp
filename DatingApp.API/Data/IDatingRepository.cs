using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> Users ();
        Task<User> GetUser(string Id);
        Task<Photo> GetPhoto(int id);
        
        Task<Photo> GetMainPhoto(string userID);

    }
}