namespace Identity.Domain.Aggregates.User
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> FindByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
    }
}
