namespace IntercontinentalExchange.Domain.Contracts.Configurations
{
    public interface IDownloadNoaaFileServiceConfig
    {
         string RootUtl { get; }
         string FileUrlPattern { get; }

         string FileNamePattern { get; }
    }
}
