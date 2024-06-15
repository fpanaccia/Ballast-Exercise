using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aircraft.Domain.Aggregates.Airplane
{
    public interface IAirplaneService
    {
        Task<Airplane> CreateAsync(Airplane airplane);
        Task<Airplane?> UpdateAsync(Airplane airplane);
        Task DeletAsync(Guid airplaneId);
        Task<Airplane?> GetByIdAsync(Guid airplaneId);
        Task<List<Airplane>> GetAllAsync();
    }
}
