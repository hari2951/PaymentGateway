using CsvHelper.Configuration;
using CsvHelper;
using ProductReport.Features.Sales.Models;
using System.Globalization;
using ProductReport.Features.Sales.Mapping;

namespace ProductReport.Features.Sales.Services
{
    public class SalesCsvParser : ICsvParser<SalesRecord>
    {
        private readonly ILogger<SalesCsvParser> _logger;

        public SalesCsvParser(ILogger<SalesCsvParser> logger) => _logger = logger;

        public List<SalesRecord> Parse(StreamReader reader)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null,
                PrepareHeaderForMatch = args => args.Header.Trim(),
                TrimOptions = TrimOptions.Trim,
                HasHeaderRecord = true,
                Delimiter = ",",
                IgnoreBlankLines = true
            };

            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<SalesRecordMap>();

            var records = new List<SalesRecord>();

            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<SalesRecord>();
                    if (record != null)
                    {
                        records.Add(record);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading CSV record at line {LineNumber}", csv.Context.Parser.Row);
                }
            }

            return records;
        }
    }
}
