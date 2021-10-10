using Microsoft.AspNetCore.Mvc;

namespace IntercontinentalExchange.Host.Bases
{
    [Route("api/[controller]")]
    [ApiController]
    //[ProducesResponseType(typeof(AppValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    //[ProducesResponseType(typeof(AppNotFoundProblemDetails), StatusCodes.Status404NotFound)]
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult Json(object response) => new JsonResult(response);
        protected IActionResult Success(string message) => new JsonResult(new { Message = message });
    }
}
