using Core.Entities;
using Core.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с вычислениями цен.
    /// </summary>
    public class CalculationRepository : ICalculationRepository
    {
        private readonly string _connectionString;
        private SqliteConnection _connection;

        /// <summary>
        /// Создает новый экземпляр класса CalculationRepository.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public CalculationRepository(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqliteConnection(_connectionString);
        }

        /// <summary>
        /// Добавляет результат вычисления в базу данных.
        /// </summary>
        /// <param name="calculation">Объект вычисления.</param>
        public async Task AddAsync(Calculation calculation)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();
            command.CommandText =
                @"
                INSERT INTO Calculations (Price, FinalPrice, TotalDiscount, SalonPath, CalculationDate)
                VALUES ($price, $finalPrice, $totalDiscount, $salonPath, $calculationDate)
                ";
            command.Parameters.AddWithValue("$price", calculation.Price);
            command.Parameters.AddWithValue("$finalPrice", calculation.FinalPrice);
            command.Parameters.AddWithValue("$totalDiscount", calculation.TotalDiscount);
            command.Parameters.AddWithValue("$salonPath", calculation.SalonPath);
            command.Parameters.AddWithValue("$calculationDate", calculation.CalculationDate);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Получает все вычисления из базы данных.
        /// </summary>
        /// <returns>Список вычислений.</returns>
        public async Task<List<Calculation>> GetAllAsync()
        {
            var calculations = new List<Calculation>();

            await _connection.OpenAsync();
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Calculations";
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var calculation = new Calculation
                {
                    Id = reader.GetInt32(0),
                    Price = reader.GetDouble(1),
                    FinalPrice = reader.GetDouble(2),
                    TotalDiscount = reader.GetDouble(3),
                    SalonPath = reader.GetString(4),
                    CalculationDate = reader.GetDateTime(5)
                };
                calculations.Add(calculation);
            }

            return calculations;
        }
    }
}