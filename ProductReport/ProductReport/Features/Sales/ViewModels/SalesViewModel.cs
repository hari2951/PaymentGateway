using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using ProductReport.Configurations;
using ProductReport.Features.Sales.Models;
using ProductReport.Features.Sales.Services;
using ProductReport.Features.Sales.Utilities;

namespace ProductReport.Features.Sales.ViewModels
{
    public class SalesViewModel(
        ISalesDataService salesDataService,
        ILogger<SalesViewModel> logger,
        IOptions<UploadSettings> uploadSettings)
    {
        private List<SalesRecord> _salesRecords = new();
        private string _selectedSegment = string.Empty;
        private string _selectedCountry = string.Empty;
        private string _selectedProduct = string.Empty;
        private string _sortField = "Date";
        private bool _sortAscending = true;
        private readonly UploadSettings _uploadSettings = uploadSettings.Value ?? new UploadSettings();

        public bool IsLoading { get; private set; }
        public string? ErrorMessage { get; private set; }
        public bool HasData => _salesRecords.Any();

        public List<SalesRecord> SalesRecords => GetFilteredAndSortedRecords();

        public string SelectedSegment
        {
            get => _selectedSegment;
            set
            {
                if (_selectedSegment != value)
                {
                    _selectedSegment = value;
                    NotifyStateChanged();
                }
            }
        }

        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    NotifyStateChanged();
                }
            }
        }

        public string SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    NotifyStateChanged();
                }
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public List<string> Segments => _salesRecords.Select(r => r.Segment).Distinct().OrderBy(s => s).ToList();
        public List<string> Countries => _salesRecords.Select(r => r.Country).Distinct().OrderBy(c => c).ToList();
        public List<string> Products => _salesRecords.Select(r => r.Product).Distinct().OrderBy(p => p).ToList();

        public decimal TotalUnitsSold => SalesRecords.Sum(r => r.UnitsSold);
        public decimal TotalManufacturingPrice => SalesRecords.Sum(r => PriceUtils.GetTotal(r.ManufacturingPrice, r.UnitsSold));
        public decimal TotalSalePrice => SalesRecords.Sum(r => PriceUtils.GetTotal(r.SalePrice, r.UnitsSold));
        public decimal TotalProfitLoss => SalesRecords.Sum(r => r.ProfitLoss);

        public void LoadDefaultData()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                NotifyStateChanged();

                logger.LogInformation("Loading default sales data...");
                _salesRecords = salesDataService.GetDefaultSalesData().ToList();
                logger.LogInformation("Loaded {Count} records", _salesRecords.Count);

                ResetFilters();
            }
            catch (FileNotFoundException)
            {
                ErrorMessage = "Default sales data file not found. Please check if the file exists.";
                _salesRecords.Clear();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load default data");
                ErrorMessage = "Failed to load default data. Please try again.";
                _salesRecords.Clear();
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public async Task ProcessUploadedFileAsync(IBrowserFile file)
        {
            if (file is null)
            {
                ErrorMessage = "No file selected.";
                NotifyStateChanged();
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = null;
                NotifyStateChanged();

                logger.LogInformation("Processing uploaded file: {FileName}", file.Name);

                if (file.Size > _uploadSettings.MaxFileSizeInMb * 1024 * 1024)
                {
                    ErrorMessage = "File size must be less than 10MB.";
                    return;
                }

                string extension = Path.GetExtension(file.Name).ToLowerInvariant();
                if (extension != ".csv")
                {
                    ErrorMessage = "Only CSV files are supported.";
                    return;
                }

                _salesRecords = (await salesDataService.ProcessUploadedFileAsync(file)).ToList();
                logger.LogInformation("Processed {Count} records from uploaded file", _salesRecords.Count);

                ResetFilters();
            }
            catch (InvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
                _salesRecords.Clear();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process uploaded file");
                ErrorMessage = "Failed to process the uploaded file. Please ensure it's a valid CSV file.";
                _salesRecords.Clear();
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        private void ResetFilters()
        {
            _selectedSegment = string.Empty;
            _selectedCountry = string.Empty;
            _selectedProduct = string.Empty;
            _sortField = "Date";
            _sortAscending = true;
        }

        public void SortBy(string field)
        {
            if (_sortField == field)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortField = field;
                _sortAscending = true;
            }
            NotifyStateChanged();
        }

        public string GetSortIcon(string field)
        {
            if (_sortField != field)
                return "bi-arrow-down-up";
            
            return _sortAscending ? "bi-arrow-up" : "bi-arrow-down";
        }

        private List<SalesRecord> GetFilteredAndSortedRecords()
        {
            try
            {
                var query = _salesRecords.AsQueryable();

                if (!string.IsNullOrEmpty(_selectedSegment))
                    query = query.Where(r => r.Segment == _selectedSegment);

                if (!string.IsNullOrEmpty(_selectedCountry))
                    query = query.Where(r => r.Country == _selectedCountry);

                if (!string.IsNullOrEmpty(_selectedProduct))
                    query = query.Where(r => r.Product == _selectedProduct);

                query = _sortField switch
                {
                    "Date" => _sortAscending ? query.OrderBy(r => r.Date) : query.OrderByDescending(r => r.Date),
                    "Segment" => _sortAscending ? query.OrderBy(r => r.Segment) : query.OrderByDescending(r => r.Segment),
                    "Country" => _sortAscending ? query.OrderBy(r => r.Country) : query.OrderByDescending(r => r.Country),
                    "Product" => _sortAscending ? query.OrderBy(r => r.Product) : query.OrderByDescending(r => r.Product),
                    "UnitsSold" => _sortAscending ? query.OrderBy(r => r.UnitsSold) : query.OrderByDescending(r => r.UnitsSold),
                    "ManufacturingPrice" => _sortAscending ? query.OrderBy(r => r.ManufacturingPrice) : query.OrderByDescending(r => r.ManufacturingPrice),
                    "SalePrice" => _sortAscending ? query.OrderBy(r => r.SalePrice) : query.OrderByDescending(r => r.SalePrice),
                    "ProfitLoss" => _sortAscending ? query.OrderBy(r => r.ProfitLoss) : query.OrderByDescending(r => r.ProfitLoss),
                    _ => query
                };

                return query.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error filtering/sorting records");
                return new List<SalesRecord>();
            }
        }
    }
} 