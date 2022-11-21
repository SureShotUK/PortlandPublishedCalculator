using System;
using System.Collections.Generic;

namespace PortlandPublishedCalculator.Prices
{
    public partial class PrimaSpotPrice
    {
        public DateOnly PublishedDate { get; set; }
        public string Grade { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public double? Price { get; set; }
    }
}
