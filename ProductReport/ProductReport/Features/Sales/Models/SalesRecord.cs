using ProductReport.Features.Sales.Utilities;

namespace ProductReport.Features.Sales.Models
{
    public class SalesRecord
    {
        public string Segment { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string DiscountBand { get; set; } = string.Empty;
        public decimal UnitsSold { get; set; }
        public string ManufacturingPrice { get; set; } = string.Empty;
        public string SalePrice { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public decimal ProfitLoss => (PriceUtils.Sanitize(SalePrice) - PriceUtils.Sanitize(ManufacturingPrice)) * UnitsSold;
    }
} 