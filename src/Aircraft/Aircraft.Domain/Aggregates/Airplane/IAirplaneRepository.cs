namespace Aircraft.Domain.Aggregates.Airplane
{
    public interface IAirplaneRepository
    {
        Task<Airplane> AddAsync(Airplane airplane);
        Task<Airplane> UpdateAsync(Airplane airplane);
        Task DeleteAsync(Airplane airplane);
        Task<Airplane?> FindByIdAsync(Guid id);
        Task<List<Airplane>> GetAllAsync();
    }
}
