using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с результами расчетов.
    /// </summary>
    public interface ICalculationRepository
    {
        /// <summary>
        /// Асинхронно добавляет результаты расчета в бд.
        /// </summary>
        /// <param name="calculation">Результат, который надо добавить.</param>
        /// <returns></returns>
        Task AddAsync(Calculation calculation);
        /// <summary>
        /// Асинхронно получает все результаты расчетов из бд.
        /// </summary>
        /// <returns></returns>
        Task<List<Calculation>> GetAllAsync();
    }
}
