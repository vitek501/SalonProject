using Core.Entities;
using Core.Interfaces;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CalculationRepository : ICalculationRepository
    {
        private readonly string _connectionString;

        public CalculationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(Calculation calculation)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
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
        }

        public async Task<List<Calculation>> GetAllAsync()
        {
            var calculations = new List<Calculation>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
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
            }

            return calculations;
        }
    }
}
