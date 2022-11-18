using System;
using System.Collections.Generic;

namespace PortlandPublishedCalculator.Prices
{
    public partial class ZIcefutLsg
    {
        public DateOnly PublishedDate { get; set; }
        public DateOnly PricingDate { get; set; }
        public double? Price { get; set; }
        public string? RelativeMonth { get; set; }
    }
}
