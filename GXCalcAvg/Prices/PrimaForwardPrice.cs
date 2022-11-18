using System;
using System.Collections.Generic;

namespace PortlandPublishedCalculator.Prices
{
    public partial class PrimaForwardPrice
    {
        public int Id { get; set; }
        public DateOnly PublishedDate { get; set; }
        public string Grade { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public string Delivery { get; set; } = null!;
        public double? Price { get; set; }
    }
}
