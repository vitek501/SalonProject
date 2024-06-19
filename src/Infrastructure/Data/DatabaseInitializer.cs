using Core.Entities;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// Инициализатор базы данных для создания таблицы салонов и заполнения начальными данными, если они отсутствуют.
    /// </summary>
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор инициализатора базы данных.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных SQLite.</param>
        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Асинхронно инициализирует базу данных: создает таблицу салонов и заполняет ее начальными данными, если они отсутствуют.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию инициализации.</returns>
        public async Task InitializeAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Создание таблицы Salons, если она не существует
                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText =
                    @"
                    CREATE TABLE IF NOT EXISTS Salons (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ParentIds TEXT,
                        ParentIdsWithIt TEXT,
                        Name TEXT NOT NULL,
                        Discount REAL NOT NULL,
                        HasDependency INTEGER NOT NULL,
                        Description TEXT NOT NULL
                    );
                    ";
                await createTableCmd.ExecuteNonQueryAsync();

                // Проверка наличия данных в таблице Salons
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM Salons";
                var count = (long)await checkCmd.ExecuteScalarAsync();

                // Если таблица пустая, заполняем ее начальными данными
                if (count == 0)
                {
                    var salons = new List<Salon>
                {
                    new Salon { Name = "Миасс", Discount = 4, HasDependency = false, Description = "Описание Миасс" },
                    new Salon { Name = "Амелия", Discount = 5, HasDependency = true, Description = "Описание Амелия", ParentIds = new int[] { 1 } },
                    new Salon { Name = "Тест1", Discount = 2, HasDependency = true, Description = "Описание Тест1", ParentIds = new int[] { 2 } },
                    new Salon { Name = "Тест2", Discount = 0, HasDependency = true, Description = "Описание Тест2", ParentIds = new int[] { 1 } },
                    new Salon { Name = "Курган", Discount = 11, HasDependency = false, Description = "Описание Курган" }
                };

                    // Вставка начальных данных в таблицу Salons
                    foreach (var salon in salons)
                    {
                        var command = connection.CreateCommand();
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
                }
            }
        }
    }
}