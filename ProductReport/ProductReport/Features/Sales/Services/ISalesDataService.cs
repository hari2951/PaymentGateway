using Microsoft.AspNetCore.Components.Forms;
using ProductReport.Features.Sales.Models;

namespace ProductReport.Features.Sales.Services
{
    public interface ISalesDataService
    {
        Task<List<SalesRecord>> GetDefaultSalesDataAsync();
        Task<List<SalesRecord>> ProcessUploadedFileAsync(IBrowserFile file);
    }
} 