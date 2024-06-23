using Core.Entities;
using Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.UseCases
{
    /// <summary>
    /// Представляет собой класс для вычисления итоговой цены с учетом скидок для определенного салона.
    /// </summary>
    public class CalculatePriceUseCase
    {
        private readonly ISalonRepository _salonRepository;
        private readonly ICalculationRepository _calculationRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CalculatePriceUseCase"/>.
        /// </summary>
        /// <param name="salonRepository">Репозиторий для салонов.</param>
        /// <param name="calculationRepository">Репозиторий для расчетов.</param>
        public CalculatePriceUseCase(ISalonRepository salonRepository, ICalculationRepository calculationRepository)
        {
            _salonRepository = salonRepository;
            _calculationRepository = calculationRepository;
        }

        /// <summary>
        /// Вычисляет итоговую цену с учетом скидок для определенного салона.
        /// </summary>
        /// <param name="price">Начальная цена без скидок.</param>
        /// <param name="salonName">Название салона для расчета цены.</param>
        /// <returns>Итоговая цена после применения скидок.</returns>
        public async Task<double> CalculatePriceAsync(double price, string salonName)
        {
            var salons = await _salonRepository.GetAllAsync();
            var selectedSalon = salons.FirstOrDefault(s => s.Name == salonName) ?? throw new ArgumentException($"Салон {salonName} не найден");

            var totalDiscount = selectedSalon.Discount;
            var salonPath = $"{selectedSalon.Name}({selectedSalon.Discount}%)";
            var currentSalon = selectedSalon;

            while (currentSalon.ParentIds.Length > 0)
            {
                var parentId = currentSalon.ParentIds.Last();
                currentSalon = salons.FirstOrDefault(s => s.Id == parentId);

                if (currentSalon != null)
                {
                    totalDiscount += currentSalon.Discount;
                    salonPath = $"{currentSalon.Name}({currentSalon.Discount}%) + {salonPath}";
                }
                else
                {
                    throw new ArgumentException($"Родительский салон с ID {parentId} не найден");
                }
            }

            var finalPrice = price - (price * (totalDiscount / 100));

            var calculation = new Calculation
            {
                Price = price,
                FinalPrice = finalPrice,
                TotalDiscount = totalDiscount,
                SalonPath = salonPath,
                CalculationDate = DateTime.Now
            };

            await _calculationRepository.AddAsync(calculation);

            return finalPrice;
        }
    }
}