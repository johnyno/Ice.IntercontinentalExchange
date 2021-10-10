using System.Threading.Tasks;

namespace IntercontinentalExchange.Domain.Contracts
{
    public interface IParseNoaaFilesService
    {
        Task<double> GetTemperature(double lon, double lat, string filePath, string parserLocation);
    }
}
