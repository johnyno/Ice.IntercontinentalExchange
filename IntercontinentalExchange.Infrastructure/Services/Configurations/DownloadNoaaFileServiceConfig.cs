using IntercontinentalExchange.Domain.Contracts.Configurations;

namespace IntercontinentalExchange.Infrastructure.Services.Configurations
{
    public class DownloadNoaaFileServiceConfig:IDownloadNoaaFileServiceConfig
    {
        public string RootUtl => @"https://noaa-gfs-bdp-pds.s3.amazonaws.com";
        public string FileUrlPattern => @"gfs.{0}{1}{2}/00/atmos";
        public string FileNamePattern => "gfs.t00z.pgrb2.0p25.f{0}";
    }
}
