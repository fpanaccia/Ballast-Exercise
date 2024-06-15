namespace Identity.Domain.Aggregates.User
{
    public interface IUserService
    {
        Task<User> CreateAsync(User user);
        Task<List<User>> GetAllAsync();

    }
}
