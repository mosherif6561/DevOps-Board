using Backend.Models;

namespace Backend.Data
{
    public interface IAuthRepository
    {
        Task<Users> Register(Users user, string password);
        Task<Users> Login(string username, string password);
        Task<bool> UserExist(string username);
        Task<Users> GetByEmail(string email);
    }
}