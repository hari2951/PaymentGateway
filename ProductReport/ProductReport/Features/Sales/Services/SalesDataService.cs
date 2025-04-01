using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using ProductReport.Configurations;
using ProductReport.Features.Sales.Utilities;
using ProductReport.Features.Sales.Models;
using System.Text;

namespace ProductReport.Features.Sales.Services
{
    public class SalesDataService(
        IWebHostEnvironment environment,
        ILogger<SalesDataService> logger,
        IOptions<UploadSettings> uploadSettings)
        : ISalesDataService
    {
        private readonly IWebHostEnvironment _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        private readonly ILogger<SalesDataService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly UploadSettings _uploadSettings = uploadSettings.Value ?? new UploadSettings();

        public IList<SalesRecord> GetDefaultSalesData()
        {
            try
            {
                _logger.LogInformation("Starting GetDefaultSalesDataAsync method");
                string defaultFilePath = Path.Combine(_environment.WebRootPath, "data", "Data.csv");
                _logger.LogInformation("Loading default sales data from: {FilePath}", defaultFilePath);
                
                if (!File.Exists(defaultFilePath))
                {
                    _logger.LogWarning("Default sales data file not found at: {FilePath}", defaultFilePath);
                    throw new FileNotFoundException("Default sales data file not found.", defaultFilePath);
                }

                _logger.LogInformation("File exists, attempting to read CSV");
                var result = ReadCsvFile(defaultFilePath);
                _logger.LogInformation("Successfully read {Count} records from CSV", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading default sales data");
                throw;
            }
        }

        public async Task<IList<SalesRecord>> ProcessUploadedFileAsync(IBrowserFile file)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(file);

                await using var stream = file.OpenReadStream(maxAllowedSize: _uploadSettings.MaxFileSizeInMb * 1024 * 1024);

                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using var reader = new StreamReader(memoryStream, Encoding.UTF8);
                return ReadCsvFromStream(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing uploaded file: {FileName}", file?.Name);
                throw;
            }
        }

        private List<SalesRecord> ReadCsvFile(string filePath)
        {
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            return ReadCsvFromStream(reader);
        }

        private List<SalesRecord> ReadCsvFromStream(StreamReader reader)
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

            try
            {
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
                        continue;
                    }
                }

                return records;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing CSV data");
                throw new InvalidOperationException("Failed to parse CSV data. Please check the file format.", ex);
            }
        }
    }

    public sealed class SalesRecordMap : ClassMap<SalesRecord>
    {
        public SalesRecordMap()
        {
            Map(m => m.Date).Name("Date").TypeConverterOption.Format("MM/dd/yyyy");
            Map(m => m.Segment).Name("Segment");
            Map(m => m.Country).Name("Country");
            Map(m => m.Product).Name("Product");
            Map(m => m.DiscountBand).Name("Discount Band");
            Map(m => m.UnitsSold)
                .Name("Units Sold")
                .TypeConverter<DecimalSanitizer>();
            Map(m => m.ManufacturingPrice).Name("Manufacturing Price");
            Map(m => m.SalePrice).Name("Sale Price");
        }
    }
} 