using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace ProductReport.Features.Sales.Utilities
{
    public class DecimalSanitizer : DecimalConverter
    {
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0m;
            }

            var sanitizedValue = text.Replace(" ", "").Trim();

            return decimal.TryParse(sanitizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0m;
        }
    }
}
