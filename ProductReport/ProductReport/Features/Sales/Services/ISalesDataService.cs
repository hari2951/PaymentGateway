using Microsoft.AspNetCore.Components.Forms;
using ProductReport.Features.Sales.Models;

namespace ProductReport.Features.Sales.Services
{
    public interface ISalesDataService
    {
        IList<SalesRecord> GetDefaultSalesData();
        Task<IList<SalesRecord>>  ProcessUploadedFileAsync(IBrowserFile file);
    }
} 