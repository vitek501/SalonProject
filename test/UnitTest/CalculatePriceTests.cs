using Core.Interfaces;
using Core.UseCases;
using Infrastructure.Data;
using Infrastructure.Repositories;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace SalonProject.Tests
{
    /// <summary>
    /// Класс, содержащий тесты для проверки правильности расчета стоимости услуги.
    /// </summary>
    [TestFixture]
    public class CalculatePriceTests
    {
        private ISalonRepository _salonRepository;
        private ICalculationRepository _calculationRepository;
        private CalculatePriceUseCase _calculatePriceUseCase;
        private string _connectionString;
        private string _dbPath;

        /// <summary>
        /// Метод, выполняемый перед каждым тестом.
        /// </summary>
        [SetUp]
        public async Task SetUp()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.db");
            _connectionString = "Data Source={_dbPath}";

            var databaseInitializer = new DatabaseInitializer(_connectionString);
            await databaseInitializer.InitializeAsync();

            _salonRepository = new SalonRepository(_connectionString);
            _calculationRepository = new CalculationRepository(_connectionString);
            _calculatePriceUseCase = new CalculatePriceUseCase(_salonRepository, _calculationRepository);
        }

        [TearDown]
        public void TearDown()
        {
            (_salonRepository as IDisposable)?.Dispose();
            (_calculationRepository as IDisposable)?.Dispose();
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }

        /// <summary>
        /// Тест, проверяющий правильность расчета стоимости услуги.
        /// </summary>
        [Test]
        [TestCase("Амелия", 5360, 4877.6)]
        [TestCase("Тест1", 136540, 121520.6)]
        [TestCase("Тест2", 54054, 51891.84)]
        [TestCase("Курган", 57850, 51486.5)]
        [TestCase("Миасс", 57470, 55171.2)]
        public async Task CalculatePrice_ShouldReturnCorrectFinalPrice(string salonName, double inputPrice, double expectedFinalPrice)
        {
            var finalPrice = await _calculatePriceUseCase.CalculatePriceAsync(inputPrice, salonName);
            Assert.That(finalPrice, Is.EqualTo(expectedFinalPrice).Within(0.01), $"Стоимость для салона '{salonName}' рассчитана неправильно. Ожидалось: {expectedFinalPrice}, получено: {finalPrice}.");
        }
    }
}