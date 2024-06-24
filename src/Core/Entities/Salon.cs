using System;

namespace Core.Entities
{
    public class Salon
    {
        public int Id { get; set; }
        public int[] ParentIds { get; set; } = Array.Empty<int>();
        public int[] ParentIdsWithIt { get; set; } = Array.Empty<int>();
        public string Name { get; set; }
        public double Discount { get; set; }
        public bool HasDependency { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}