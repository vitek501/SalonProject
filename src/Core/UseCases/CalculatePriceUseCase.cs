using Core.Entities;
using Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.UseCases
{
    public class CalculatePriceUseCase
    {
        private readonly ISalonRepository _salonRepository;
        private readonly ICalculationRepository _calculationRepository;

        public CalculatePriceUseCase(ISalonRepository salonRepository, ICalculationRepository calculationRepository)
        {
            _salonRepository = salonRepository;
            _calculationRepository = calculationRepository;
        }

        public async Task<double> CalculatePriceAsync(double price, string salonName)
        {
            var salons = await _salonRepository.GetAllAsync();
            var selectedSalon = salons.FirstOrDefault(s => s.Name == salonName) ?? throw new ArgumentException($"Салон не найден"); ;

            var totalDiscount = selectedSalon.Discount;
            var salonPath = selectedSalon.Name + "(" + selectedSalon.Discount + "%)";
            var currentSalon = selectedSalon;

            while (currentSalon.ParentIds.Length > 0)
            {
                var parentId = currentSalon.ParentIds.Last();
                currentSalon = salons.FirstOrDefault(s => s.Id == parentId);

                if (currentSalon != null)
                {
                    totalDiscount += currentSalon.Discount;
                    salonPath = (currentSalon.Name + "(" + currentSalon.Discount + "%)") + " + " + salonPath;
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
