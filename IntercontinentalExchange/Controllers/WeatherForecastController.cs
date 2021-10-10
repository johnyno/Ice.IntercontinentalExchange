using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IntercontinentalExchange.Application.Interactors;
using IntercontinentalExchange.Domain.Contracts;
using IntercontinentalExchange.Host.Bases;
using IntercontinentalExchange.Host.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntercontinentalExchange.Host.Controllers
{
  
    public class WeatherForecastController : ApiControllerBase
    {
        private readonly IAppLogger _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(IAppLogger logger,IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost(nameof(GetForecast))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForecast(
            [FromBody, Required] GetForecastRequest request)
        {
            _logger.Debug("Request processing started");
            var response = await _mediator.Send(new GetForecastInteractorRequest(request.Date, request.Lat, request.Lon));
            _logger.Debug($"Request processing finished with result {response.Temperature}");

            return Success($"Temperature at requested place is {response.Temperature}");
        }

     
    }
}
