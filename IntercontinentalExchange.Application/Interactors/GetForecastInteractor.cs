using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntercontinentalExchange.Domain.Contracts;
using IntercontinentalExchange.Domain.Handlers;
using MediatR;

namespace IntercontinentalExchange.Application.Interactors
{

    public class GetForecastInteractorRequest : IRequest<GetForecastInteractorResponse>
    {
        public DateTime Date { get; }
        public double Lat { get; }
        public double Lon { get; }

        public GetForecastInteractorRequest(DateTime date, double lat, double lon)
        {
            Date = date;
            Lat = lat;
            Lon = lon;
        }
    }

    public class GetForecastInteractorResponse
    {
        public GetForecastInteractorResponse(double temperature)
        {
            Temperature = temperature;
        }

        public double Temperature { get; }
    }



    internal class GetForecastInteractor : IRequestHandler<GetForecastInteractorRequest, GetForecastInteractorResponse>
    {
        private readonly IAppLogger _logger;
        private readonly IMediator _mediator;

        public GetForecastInteractor(IMediator mediator, IAppLogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<GetForecastInteractorResponse> Handle(GetForecastInteractorRequest request, CancellationToken cancellationToken)
        {
            _logger.Debug("Start handling iterator request");
            //todo: pass it by config
            var destinationFolder = @"c:\temp";
            var parserLocation = @"C:\temp\parser\wgrib2.exe";
            var downloadFileResponse =
                await _mediator.Send(new DownloadForecastFileHandlerRequest(request.Date, destinationFolder), cancellationToken);



            var parseFileResponse = await _mediator.Send(
                new ParseFileHandlerRequest(request.Lat
                                            , request.Lon
                                            , $"{destinationFolder}\\{downloadFileResponse.FileName}"
                                            , parserLocation)
                , cancellationToken);

            return await Task.FromResult(new GetForecastInteractorResponse(parseFileResponse.Temperature));
        }
    }
}
