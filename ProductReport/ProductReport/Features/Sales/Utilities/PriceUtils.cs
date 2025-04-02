using System.Globalization;
using System.Text.RegularExpressions;

namespace ProductReport.Features.Sales.Utilities
{
    public static class PriceUtils
    {
        public static decimal GetTotal(string priceString, decimal? units)
        {
            if (string.IsNullOrWhiteSpace(priceString) || !units.HasValue)
                return 0;

            return Sanitize(priceString) * units.Value;
        }

        public static decimal Sanitize(string priceString)
        {
            if (string.IsNullOrWhiteSpace(priceString))
            {
                return 0;
            }

            var cleaned = priceString.Trim();

            cleaned = Regex.Replace(cleaned, @"[^\d,\.]", "");

            if (Regex.IsMatch(cleaned, @"^\d{1,3}(?:\.\d{3})*,\d{1,2}$") || cleaned.Contains(',') && !cleaned.Contains('.'))
            {
                cleaned = cleaned.Replace(".", "").Replace(",", ".");
            }
            else
            {
                cleaned = cleaned.Replace(",", "");
            }

            return decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) ? price : 0;
        }
    }
}
