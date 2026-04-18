using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
    }
}
