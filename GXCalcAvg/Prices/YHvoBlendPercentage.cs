using System;
using System.Collections.Generic;

namespace PortlandPublishedCalculator.Prices
{
    public partial class YHvoBlendPercentage
    {
        public DateOnly PublishedDate { get; set; }
        public double? Diesel { get; set; }
        public double? Fame { get; set; }
        public double? Hvo { get; set; }
    }
}
