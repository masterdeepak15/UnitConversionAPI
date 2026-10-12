using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Dtos;
using UnitConversion.Core.Abstractions;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/conversions")]
public sealed class ConversionsController(IUnitConverter converter) : ControllerBase
{
    /// <summary>Converts a number from one unit to another, e.g. /api/conversions?value=100&amp;from=celsius&amp;to=fahrenheit.</summary>
    /// <param name="value">The number to convert.</param>
    /// <param name="from">Source unit: id, name, symbol or alias.</param>
    /// <param name="to">Target unit: id, name, symbol or alias.</param>
    [HttpGet]
    [ProducesResponseType<ConversionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public ActionResult<ConversionResponse> Convert(
        [FromQuery] decimal value,
        [FromQuery] string from,
        [FromQuery] string to) =>
        ConversionResponse.Map(converter.Convert(value, from, to));
}
