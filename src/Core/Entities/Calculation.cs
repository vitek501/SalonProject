using System;

namespace Core.Entities
{
    public class Calculation
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }
        public double TotalDiscount { get; set; }
        public string SalonPath { get; set; }
        public DateTime CalculationDate { get; set; }
    }
}
