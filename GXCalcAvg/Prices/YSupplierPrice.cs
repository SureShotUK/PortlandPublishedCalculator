using System;
using System.Collections.Generic;

namespace PortlandPublishedCalculator.Prices
{
    /// <summary>
    /// This table houses the prices that we receive from suppliers
    /// </summary>
    public partial class YSupplierPrice
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public DateOnly PublishedDate { get; set; }
        public int GradeId { get; set; }
        public double Price { get; set; }
    }
}
