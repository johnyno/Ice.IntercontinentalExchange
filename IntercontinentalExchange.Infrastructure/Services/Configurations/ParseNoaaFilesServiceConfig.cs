using IntercontinentalExchange.Domain.Contracts.Configurations;

namespace IntercontinentalExchange.Infrastructure.Services.Configurations
{
    public class ParseNoaaFilesServiceConfig:IParseNoaaFilesServiceConfig
    {
        public string ArgumentsPattern => "{0} -match \":(TMP:2 m above ground):\" -lon {1} {2}";
    }
}
