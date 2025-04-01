using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;
using ProductReport.Features.Sales.Models;
using ProductReport.Features.Sales.Services;

namespace ProductReport.Features.Sales.ViewModels
{
    public class SalesViewModel
    {
        private readonly ISalesDataService _salesDataService;
        private List<SalesRecord> _salesRecords = new();
        private string _selectedSegment = string.Empty;
        private string _selectedCountry = string.Empty;
        private string _selectedProduct = string.Empty;
        private string _sortField = "Date";
        private bool _sortAscending = true;

        public SalesViewModel(ISalesDataService salesDataService)
        {
            _salesDataService = salesDataService;
        }

        public List<SalesRecord> SalesRecords => GetFilteredAndSortedRecords();
        public string SelectedSegment
        {
            get => _selectedSegment;
            set => _selectedSegment = value;
        }
        public string SelectedCountry
        {
            get => _selectedCountry;
            set => _selectedCountry = value;
        }
        public string SelectedProduct
        {
            get => _selectedProduct;
            set => _selectedProduct = value;
        }

        public List<string> Segments => _salesRecords.Select(r => r.Segment).Distinct().ToList();
        public List<string> Countries => _salesRecords.Select(r => r.Country).Distinct().ToList();
        public List<string> Products => _salesRecords.Select(r => r.Product).Distinct().ToList();

        public decimal TotalUnitsSold => SalesRecords.Sum(r => r.UnitsSold);
        public decimal TotalManufacturingPrice => SalesRecords.Sum(r => r.ManufacturingPrice * r.UnitsSold);
        public decimal TotalSalePrice => SalesRecords.Sum(r => r.SalePrice * r.UnitsSold);
        public decimal TotalProfitLoss => SalesRecords.Sum(r => r.ProfitLoss);

        public async Task LoadDefaultDataAsync()
        {
            _salesRecords = await _salesDataService.GetDefaultSalesDataAsync();
        }

        public async Task ProcessUploadedFileAsync(IBrowserFile file)
        {
            _salesRecords = await _salesDataService.ProcessUploadedFileAsync(file);
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
        }

        private List<SalesRecord> GetFilteredAndSortedRecords()
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
    }
} 