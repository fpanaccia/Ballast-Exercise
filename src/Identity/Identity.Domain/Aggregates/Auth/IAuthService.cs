namespace Identity.Domain.Aggregates.User
{
    public interface IAuthService
    {
        Task<string> LoginAttempt(string email, string password);
    }
}
