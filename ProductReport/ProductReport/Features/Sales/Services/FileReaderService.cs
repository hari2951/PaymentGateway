using Microsoft.AspNetCore.Components.Forms;
using System.Text;

namespace ProductReport.Features.Sales.Services
{
    public class FileReaderService : IFileReaderService
    {
        public async Task<StreamReader> ReadUploadedFileAsync(IBrowserFile file, int maxSizeInBytes)
        {
            var stream = file.OpenReadStream(maxSizeInBytes);
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return new StreamReader(memoryStream, Encoding.UTF8);
        }

        public async Task<StreamReader> ReadAsStreamReaderAsync(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new StreamReader(stream, Encoding.UTF8);
        }
    }
}
