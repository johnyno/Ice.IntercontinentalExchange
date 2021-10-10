using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using IntercontinentalExchange.Domain.Contracts;

namespace IntercontinentalExchange.Infrastructure.Services
{
    internal class DownloadFileService : IDownloadFileService
    {
        private readonly IAppLogger _logger;

        public DownloadFileService(IAppLogger logger)
        {
            _logger = logger;
        }
        public async Task DownloadFile(string url, string folder)
        {
            try
            {
                var httpClient = new HttpClient();

                var httpResponse = await httpClient.GetAsync(url);

                 await using var fs = File.Create(folder);

                 await httpResponse.Content.CopyToAsync(fs);
            }
            catch (Exception e)
            {
                _logger.Error("File download error",e);
                throw;
            }

        }

        public bool IsFileExist(string path)
        {
            var isFileExist = File.Exists(path);
            return isFileExist;
        }
    }
}
