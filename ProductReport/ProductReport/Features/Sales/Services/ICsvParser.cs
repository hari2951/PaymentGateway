namespace ProductReport.Features.Sales.Services
{
    public interface ICsvParser<T>
    {
        List<T> Parse(StreamReader reader);
    }
}
