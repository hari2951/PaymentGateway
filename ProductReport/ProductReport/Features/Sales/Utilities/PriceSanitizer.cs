using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ProductReport.Features.Sales.Utilities
{
    public class PriceSanitizer : StringConverter
    {
        private static readonly string DefaultSymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;

        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return DefaultSymbol + "0.00";

            var trimmed = text.Trim();

            if (char.IsDigit(trimmed[0]))
                return DefaultSymbol + trimmed;

            if (!char.IsDigit(trimmed[0]))
            {
                var cleaned = Regex.Replace(trimmed, @"^[^\d]+", DefaultSymbol);
                return cleaned;
            }

            return DefaultSymbol + "0.00";
        }
    }
}
