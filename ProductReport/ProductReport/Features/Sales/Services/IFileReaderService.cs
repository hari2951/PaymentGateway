using Microsoft.AspNetCore.Components.Forms;

namespace ProductReport.Features.Sales.Services
{
    public interface IFileReaderService
    {
        Task<StreamReader> ReadAsStreamReaderAsync(string path);
        Task<StreamReader> ReadUploadedFileAsync(IBrowserFile file, int maxSizeInBytes);
    }
}
