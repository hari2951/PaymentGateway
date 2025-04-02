using ProductReport.Features.Sales.Utilities;

namespace ProductReport.Tests.Utilities;

public class PriceUtilsTests
{
    [Theory]
    [InlineData("£100.50", 100.50)]
    [InlineData("€3.000,25", 3000.25)]
    [InlineData("1,234.56", 1234.56)]
    [InlineData("1.234,56", 1234.56)]
    [InlineData("3 456,78", 3456.78)]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData("abc", 0)]
    public void Sanitize_ReturnsExpectedDecimal(string input, decimal expected)
    {
        var result = PriceUtils.Sanitize(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(GetTotalTestData))]
    public void GetTotal_CalculatesCorrectly(string priceString, decimal? units, decimal expected)
    {
        var result = PriceUtils.GetTotal(priceString, units);
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> GetTotalTestData => new List<object[]>
    {
        new object[] { "£100", 2m, 200m },
        new object[] { "€3.000,25", 1m, 3000.25m },
        new object[] { "100.5", null, 0m },
        new object[] { null, 3m, 0m },
        new object[] { "invalid", 3m, 0m },
    };
}