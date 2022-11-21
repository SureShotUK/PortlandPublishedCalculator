using System;
using System.Collections.Generic;

namespace PortlandPublishedCalculator.Prices
{
    public partial class YArgusomrThg
    {
        public DateOnly PublishedDate { get; set; }
        public string Grade { get; set; } = null!;
        public double? Low { get; set; }
        public double? High { get; set; }
    }
}
