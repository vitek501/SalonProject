using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICalculationRepository
    {
        Task AddAsync(Calculation calculation);
        Task<List<Calculation>> GetAllAsync();
    }
}
