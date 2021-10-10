using System;
using System.Threading;
using System.Threading.Tasks;
using IntercontinentalExchange.Domain.Bases;
using IntercontinentalExchange.Domain.Contracts;
using MediatR;

namespace IntercontinentalExchange.Domain.Handlers
{
    public class ParseFileHandlerRequest:IRequest<ParseFileHandlerResponse>
    {
        public ParseFileHandlerRequest(double lat, double lon, string filePath, string parserLocation)
        {
            Lat = lat;
            Lon = lon;
            FilePath = filePath;
            ParserLocation = parserLocation;
        }
        public double Lat { get; }
        public double Lon { get; }
        public string FilePath { get;}

        public string ParserLocation { get; set; }
    }

    public class ParseFileHandlerResponse
    {
        public double Temperature { get; set; }
    }

    internal class ParseFileHandler: HandlerBase, IRequestHandler<ParseFileHandlerRequest, ParseFileHandlerResponse>
    {
        private readonly IParseNoaaFilesService _parseNoaaFilesService;

        public ParseFileHandler(IAppLogger logger, IParseNoaaFilesService parseNoaaFilesService) : base(logger)
        {
            _parseNoaaFilesService = parseNoaaFilesService;
        }

        public async Task<ParseFileHandlerResponse> Handle(ParseFileHandlerRequest request, CancellationToken cancellationToken)
        {
            //todo: add log
            var result = await _parseNoaaFilesService.GetTemperature(request.Lon, request.Lat, request.FilePath, request.ParserLocation);

            var celsiusTemperature = ToCelsius(result);
            var response = new ParseFileHandlerResponse() {Temperature = celsiusTemperature };

            return response;
        }

        private double ToCelsius(double kelvinTemperature)
        {
            var celsiusTemperature = Math.Round(kelvinTemperature - 273.15, 2);
            return celsiusTemperature;
        }
    }
}
