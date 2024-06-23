using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Core.Interfaces;
using Core.UseCases;


namespace SalonProject
{
    class Program
    {
        private static readonly string _connectionString = "Data Source=salon.db";
        /// <summary>
        /// Точка входа приложения.
        /// </summary>
        static async Task Main()
        {
            // Инициализация базы данных
            var databaseInitializer = new DatabaseInitializer(_connectionString);
            await databaseInitializer.InitializeAsync();

            ISalonRepository salonRepository = new SalonRepository(_connectionString);
            ICalculationRepository calculationRepository = new CalculationRepository(_connectionString);

            Console.WriteLine("Список салонов:");
            var salons = await salonRepository.GetAllAsync();
            PrintSalonsWithLevels(salons);


            var calculatePriceUseCase = new CalculatePriceUseCase(salonRepository, calculationRepository);

            // Запрос цены у пользователя
            Console.WriteLine("Введите цену:");
            var priceInput = Console.ReadLine();
            if (!double.TryParse(priceInput, out double price))
            {
                Console.WriteLine("Неверный формат цены.");
                return;
            }

            Console.WriteLine("Введите имя салона:");
            var salonName = Console.ReadLine();

            try
            {
                var finalPrice = await calculatePriceUseCase.CalculatePriceAsync(price, salonName);
                Console.WriteLine($"Итоговая цена: {finalPrice}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        /// <summary>
        /// Выводит список салонов с отображением уровня вложенности.
        /// </summary>
        /// <param name="salons">Список салонов для вывода.</param>
        static void PrintSalonsWithLevels(List<Salon> salons)
        {
            var rootSalons = salons.Where(s => s.ParentIds.Length == 0).ToList();
            foreach (var salon in rootSalons)
            {
                PrintSalon(salon, salons, 0);
            }
        }

        /// <summary>
        /// Рекурсивно выводит информацию о салоне и его дочерних салонах.
        /// </summary>
        /// <param name="salon">Салон для вывода информации.</param>
        /// <param name="allSalons">Список всех салонов.</param>
        /// <param name="level">Уровень вложенности текущего салона.</param>
        static void PrintSalon(Salon salon, List<Salon> allSalons, int level)
        {
            Console.WriteLine($"{new string(' ', level * 2)} {salon.Name} (Скидка: {salon.Discount}%, {(salon.HasDependency? "зависимость есть" : "зависимости нет")})");
            var childSalons = allSalons.Where(s => s.ParentIds.Contains(salon.Id)).ToList();
            foreach (var childSalon in childSalons)
            {
                PrintSalon(childSalon, allSalons, level + 1);
            }
        }
    }
}

