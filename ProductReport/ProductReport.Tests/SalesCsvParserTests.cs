using Moq;
using Microsoft.Extensions.Logging;
using System.Text;
using ProductReport.Features.Sales.Services;

namespace ProductReport.Tests.Parsers;

public class SalesCsvParserTests
{
    private readonly SalesCsvParser _parser;

    public SalesCsvParserTests()
    {
        var logger = Mock.Of<ILogger<SalesCsvParser>>();
        _parser = new SalesCsvParser(logger);
    }

    [Fact]
    public void Parse_ValidCsv_ReturnsRecords()
    {
        // Arrange
        var csv = """
            Date,Segment,Country,Product,Discount Band,Units Sold,Manufacturing Price,Sale Price
            01/01/2024,Government,UK,Product A,High,100,£2.00,£5.00
            01/02/2024,Private,France,Product B,Low,200,€3.00,€6.00
            """;

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        var reader = new StreamReader(stream);

        // Act
        var records = _parser.Parse(reader);

        // Assert
        Assert.Equal(2, records.Count);
        Assert.Equal("UK", records[0].Country);
        Assert.Equal("France", records[1].Country);
    }

    [Fact]
    public void Parse_InvalidUnitCount_MapAsZero()
    {
        // Arrange: Second row has an invalid Units Sold value
        var csv = """
            Date,Segment,Country,Product,Discount Band,Units Sold,Manufacturing Price,Sale Price
            01/01/2024,Government,UK,Product A,High,100,£2.00,£5.00
            01/02/2024,Private,France,Product B,Low,INVALID,€3.00,€6.00
            """;

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        var reader = new StreamReader(stream);

        // Act
        var records = _parser.Parse(reader);

        // Assert
        Assert.Equal(2, records.Count);
        Assert.Equal("UK", records[0].Country);
        Assert.Equal(0, records[1].UnitsSold);
    }

    [Fact]
    public void Parse_EmptyFile_ReturnsEmptyList()
    {
        // Arrange
        var csv = "";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        var reader = new StreamReader(stream);

        // Act
        var records = _parser.Parse(reader);

        // Assert
        Assert.Empty(records);
    }
}
