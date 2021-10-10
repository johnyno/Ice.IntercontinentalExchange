using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IntercontinentalExchange.Domain.Contracts;
using IntercontinentalExchange.Domain.Contracts.Configurations;

[assembly: InternalsVisibleTo("IntercontinentalExchange.UnitTests")]
namespace IntercontinentalExchange.Infrastructure.Services
{

    internal class DownloadNoaaFileService : IDownloadNoaaFilesService
    {
        private readonly IAppLogger _logger;
        private readonly IDownloadFileService _downloadFileService;

        private readonly string _rootUtl;
        private readonly string _fileUrlPattern;
        private readonly string _fileNamePattern;


        public DownloadNoaaFileService(IAppLogger logger, IDownloadFileService downloadFileService, IDownloadNoaaFileServiceConfig config)
        {
            _logger = logger;
            _downloadFileService = downloadFileService;

            _rootUtl = config.RootUtl;
            _fileUrlPattern = config.FileUrlPattern;
            _fileNamePattern = config.FileNamePattern;
        }

        public async Task<string> DownloadFile(DateTime date, string folderPath, bool reuseDownloadedFiles = true)
        {
            try
            {
                var formattedDateTime = CalculateOffset(date);

                var fileUrl = string.Format(_fileUrlPattern, formattedDateTime.Year, formattedDateTime.Month, formattedDateTime.Day);
                var fileName = string.Format(_fileNamePattern, formattedDateTime.Offset);
                var fullFileUrl = $"{_rootUtl}/{fileUrl}/{fileName}";


                var fullFileName = $"{folderPath}\\{fileName}";

                if (reuseDownloadedFiles && _downloadFileService.IsFileExist(fullFileName))
                {
                    return fileName;
                }

                await _downloadFileService.DownloadFile(fullFileUrl, fullFileName);


                return fileName;
            }
            catch (Exception e)
            {
                _logger.Error("Noaa file download error", e);
                throw;
            }

        }

        private NoaaDateTime CalculateOffset(DateTime date)
        {
            var today = DateTime.UtcNow;

            if (date < today)
            {
                var ret = new NoaaDateTime(date, date.TimeOfDay.Hours);
                return ret;
            }
            else
            {
                var offset = (date - today).TotalHours;
                var ret = new NoaaDateTime(today, (int)Math.Round(offset));
                return ret;
            }
        }

        private class NoaaDateTime
        {
            public NoaaDateTime(string year, string month, string day, int offset)
            {
                Year = year;
                Month = month;
                Day = day;
                Offset = offset.ToString("D3");
            }

            public NoaaDateTime(DateTime date, int offset) :
                this(date.Year.ToString(), $"{date.Month:00}", $"{date.Day:00}", offset)
            { }


            public string Year { get; }
            public string Month { get; }
            public string Day { get; }
            public string Offset { get; }

        }
    }
}
