using Moq;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using ProductReport.Configurations;
using ProductReport.Features.Sales.Models;
using ProductReport.Features.Sales.Services;
using Microsoft.AspNetCore.Hosting;

namespace ProductReport.Tests.Services;

public class SalesDataServiceTests
{
    private readonly Mock<IWebHostEnvironment> _envMock = new();
    private readonly Mock<ILogger<SalesDataService>> _loggerMock = new();
    private readonly Mock<ICsvParser<SalesRecord>> _parserMock = new();
    private readonly Mock<IFileReaderService> _readerMock = new();

    private SalesDataService CreateService(string webRootPath = "")
    {
        _envMock.Setup(e => e.WebRootPath).Returns(webRootPath);
        var options = Options.Create(new UploadSettings { MaxFileSizeInMb = 5 });

        return new SalesDataService(
            _envMock.Object,
            options,
            _parserMock.Object,
            _readerMock.Object
        );
    }

    [Fact]
    public void GetDefaultSalesData_FileExists_ReturnsParsedRecords()
    {
        // Arrange
        var webRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var dataDir = Path.Combine(webRoot, "data");
        Directory.CreateDirectory(dataDir);

        var filePath = Path.Combine(dataDir, "Data.csv");
        var fileContent = "Date,Segment,Country,Product,Discount Band,Units Sold,Manufacturing Price,Sale Price\n" +
                          "01/01/2024,Government,UK,Product A,High,100,£2.00,£5.00";
        File.WriteAllText(filePath, fileContent);

        var reader = new StreamReader(filePath);
        _readerMock.Setup(r => r.ReadAsStreamReaderAsync(filePath)).ReturnsAsync(reader);
        _parserMock.Setup(p => p.Parse(It.IsAny<StreamReader>()))
            .Returns(new List<SalesRecord> { new() { Country = "UK" } });

        var service = CreateService(webRoot);

        // Act
        var result = service.GetDefaultSalesData();

        // Assert
        Assert.Single(result);
        Assert.Equal("UK", result[0].Country);

        // Cleanup
        Directory.Delete(webRoot, true);
    }

    [Fact]
    public void GetDefaultSalesData_FileNotFound_Throws()
    {
        // Arrange
        var service = CreateService("C:\\NonExistentPath");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => service.GetDefaultSalesData());
    }

    [Fact]
    public async Task ProcessUploadedFileAsync_ReturnsParsedRecords()
    {
        // Arrange
        var mockFile = new Mock<IBrowserFile>();
        var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("Some,Data")));

        _readerMock.Setup(r => r.ReadUploadedFileAsync(mockFile.Object, It.IsAny<int>()))
            .ReturnsAsync(reader);

        _parserMock.Setup(p => p.Parse(reader))
            .Returns(new List<SalesRecord> { new() { Product = "Laptop" } });

        var service = CreateService();

        // Act
        var result = await service.ProcessUploadedFileAsync(mockFile.Object);

        // Assert
        Assert.Single(result);
        Assert.Equal("Laptop", result[0].Product);
    }
}