using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using ProductReport.Configurations;
using ProductReport.Features.Sales.Models;

namespace ProductReport.Features.Sales.Services
{
    public class SalesDataService(
        IWebHostEnvironment env,
        IOptions<UploadSettings> settings,
        ICsvParser<SalesRecord> parser,
        IFileReaderService reader)
        : ISalesDataService
    {
        private readonly UploadSettings _settings = settings.Value;

        public IList<SalesRecord> GetDefaultSalesData()
        {
            var path = Path.Combine(env.WebRootPath, "data", "Data.csv");

            if (!File.Exists(path))
                throw new FileNotFoundException("Default sales data file not found.", path);

            using var reader1 = reader.ReadAsStreamReaderAsync(path).Result;
            return parser.Parse(reader1);
        }

        public async Task<IList<SalesRecord>> ProcessUploadedFileAsync(IBrowserFile file)
        {
            var reader1 = await reader.ReadUploadedFileAsync(file, _settings.MaxFileSizeInMb * 1024 * 1024);
            return parser.Parse(reader1);
        }
    }
} 