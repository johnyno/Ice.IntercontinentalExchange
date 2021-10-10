using System;
using System.Threading;
using System.Threading.Tasks;
using IntercontinentalExchange.Domain.Bases;
using IntercontinentalExchange.Domain.Contracts;
using MediatR;

namespace IntercontinentalExchange.Domain.Handlers
{


    public class DownloadForecastFileHandlerRequest : IRequest<DownloadForecastFileHandlerResponse>
    {
        public DateTime Date { get; }
        public string DestinationPath { get; }

        public DownloadForecastFileHandlerRequest(DateTime date, string destinationPath)
        {
            Date = date;
            DestinationPath = destinationPath;
        }

    }

    public class DownloadForecastFileHandlerResponse
    {
        public DownloadForecastFileHandlerResponse(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }

    internal class DownloadForecastFileHandler : HandlerBase,
        IRequestHandler<DownloadForecastFileHandlerRequest, DownloadForecastFileHandlerResponse>
    {
        private readonly IDownloadNoaaFilesService _downloadNoaaFilesService;

        public DownloadForecastFileHandler(IAppLogger logger, IDownloadNoaaFilesService downloadNoaaFilesService) : base(logger)
        {
            _downloadNoaaFilesService = downloadNoaaFilesService;
        }

        public async Task<DownloadForecastFileHandlerResponse> Handle(DownloadForecastFileHandlerRequest request, CancellationToken cancellationToken)
        {
            var date = request.Date;
            var destination = request.DestinationPath;

            Logger.Info($"Start handling downloading forecast file request for date {date}, to destination {destination}");
            var fileName = await _downloadNoaaFilesService.DownloadFile(date, destination);
            Logger.Info($"Got file. Name: {fileName}");

            return await Task.FromResult(new DownloadForecastFileHandlerResponse(fileName));
        }
    }
}
