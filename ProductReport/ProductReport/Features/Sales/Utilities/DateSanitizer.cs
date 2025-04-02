using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace ProductReport.Features.Sales.Utilities
{
    public class DateSanitizer(string format = "MM/dd/yyyy", DateTime? defaultDate = null) : DateTimeConverter
    {
        private readonly DateTime _defaultDate = defaultDate ?? DateTime.MinValue;

        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            return DateTime.TryParseExact(text?.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed) ? parsed : _defaultDate;
        }
    }
}
