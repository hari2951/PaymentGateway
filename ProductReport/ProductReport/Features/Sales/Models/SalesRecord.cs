using System;

namespace ProductReport.Features.Sales.Models
{
    public class SalesRecord
    {
        public string Segment { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string DiscountBand { get; set; } = string.Empty;
        public decimal UnitsSold { get; set; }
        public decimal ManufacturingPrice { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime Date { get; set; }

        // Calculated property for profit/loss
        public decimal ProfitLoss => (SalePrice - ManufacturingPrice) * UnitsSold;
    }
} 