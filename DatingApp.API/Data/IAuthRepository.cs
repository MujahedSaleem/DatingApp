using System.Threading.Tasks;
using DatingApp.API.Models;
namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string Password);
        Task<User> Login(string userName, string Password);
        bool userExistsAsync(string userName);


    }
}