using CsvHelper.Configuration;
using ProductReport.Features.Sales.Models;
using ProductReport.Features.Sales.Utilities;

namespace ProductReport.Features.Sales.Mapping
{
    public class SalesRecordMap : ClassMap<SalesRecord>
    {
        public SalesRecordMap()
        {
            Map(m => m.Date).Name("Date").TypeConverter(new DateSanitizer("MM/dd/yyyy"));
            Map(m => m.Segment).Name("Segment");
            Map(m => m.Country).Name("Country");
            Map(m => m.Product).Name("Product");
            Map(m => m.DiscountBand).Name("Discount Band");
            Map(m => m.UnitsSold)
                .Name("Units Sold")
                .TypeConverter<DecimalSanitizer>();
            Map(m => m.ManufacturingPrice).Name("Manufacturing Price").TypeConverter<PriceSanitizer>();
            Map(m => m.SalePrice).Name("Sale Price").TypeConverter<PriceSanitizer>();
        }
    }
}
