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
        /// Асинхронно добавляет новый салон в хранилище.
        /// </summary>
        /// <param name="salon">Салон для добавления.</param>
        /// <returns>Задача, представляющая операцию добавления.</returns>
        Task AddAsync(Salon salon);
        /// <summary>
        /// Асинхронно получает все салоны из хранилища.
        /// </summary>
        /// <returns>Список всех салонов.</returns>
        Task<List<Salon>> GetAllAsync();
    }
}