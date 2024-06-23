using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с салонами.
    /// </summary>
    public interface ISalonRepository
    {
        /// <summary>
        /// Асинхронно добавляет новый салон в бд.
        /// </summary>
        /// <param name="salon">Салон для добавления.</param>
        Task AddAsync(Salon salon);
        /// <summary>
        /// Асинхронно получает все салоны из бд.
        /// </summary>
        /// <returns>Список всех салонов.</returns>
        Task<List<Salon>> GetAllAsync();
    }
}