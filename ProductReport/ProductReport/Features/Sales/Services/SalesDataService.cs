using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.AspNetCore.Components.Forms;
using ProductReport.Features.Sales.Models;

namespace ProductReport.Features.Sales.Services
{
    public class SalesDataService : ISalesDataService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<SalesDataService> _logger;

        public SalesDataService(
            IWebHostEnvironment environment,
            ILogger<SalesDataService> logger)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<SalesRecord>> GetDefaultSalesDataAsync()
        {
            try
            {
                string defaultFilePath = Path.Combine(_environment.WebRootPath, "data", "sales.csv");
                return await ReadCsvFileAsync(defaultFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading default sales data");
                throw new ApplicationException("Failed to load default sales data", ex);
            }
        }

        public async Task<List<SalesRecord>> ProcessUploadedFileAsync(IBrowserFile file)
        {
            try
            {
                if (file == null)
                    throw new ArgumentNullException(nameof(file));

                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                
                return csv.GetRecords<SalesRecord>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing uploaded file");
                throw new ApplicationException("Failed to process uploaded file", ex);
            }
        }

        private async Task<List<SalesRecord>> ReadCsvFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("File not found: {FilePath}", filePath);
                return new List<SalesRecord>();
            }

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            return csv.GetRecords<SalesRecord>().ToList();
        }
    }
} 