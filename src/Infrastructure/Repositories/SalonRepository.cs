using Core.Entities;
using Core.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с данными салона.
    /// </summary>
    public class SalonRepository : ISalonRepository
    {
        private readonly string _connectionString;
        private SqliteConnection _connection;

        /// <summary>
        /// Создает новый экземпляр класса SalonRepository.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public SalonRepository(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqliteConnection(_connectionString);
        }

        /// <summary>
        /// Добавляет новый салон в базу данных.
        /// </summary>
        /// <param name="salon">Объект салона, который нужно добавить.</param>
        /// <returns>Задача, завершающаяся после завершения операции добавления.</returns>
        public async Task AddAsync(Salon salon)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();
            command.CommandText =
                @"
                INSERT INTO Salons (ParentIds, ParentIdsWithIt, Name, Discount, HasDependency, Description)
                VALUES ($parentIds, $parentIdsWithIt, $name, $discount, $hasDependency, $description)
                ";
            command.Parameters.AddWithValue("$parentIds", string.Join(",", salon.ParentIds));
            command.Parameters.AddWithValue("$parentIdsWithIt", string.Join(",", salon.ParentIdsWithIt));
            command.Parameters.AddWithValue("$name", salon.Name);
            command.Parameters.AddWithValue("$discount", salon.Discount);
            command.Parameters.AddWithValue("$hasDependency", salon.HasDependency);
            command.Parameters.AddWithValue("$description", salon.Description);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Получает все салоны из базы данных.
        /// </summary>
        /// <returns>Задача, завершающаяся список салонов.</returns>
        public async Task<List<Salon>> GetAllAsync()
        {
            var salons = new List<Salon>();

            await _connection.OpenAsync();
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Salons ORDER BY ParentIdsWithIt";
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var salon = new Salon
                {
                    Id = reader.GetInt32(0),
                    ParentIds = reader.IsDBNull(1) ? Array.Empty<int>() : reader.GetString(1).Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray(),
                    ParentIdsWithIt = reader.IsDBNull(2) ? Array.Empty<int>() : reader.GetString(2).Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray(),
                    Name = reader.GetString(3),
                    Discount = reader.GetDouble(4),
                    HasDependency = reader.GetBoolean(5),
                    Description = reader.GetString(6)
                };
                salons.Add(salon);
            }

            return salons;
        }
    }
}