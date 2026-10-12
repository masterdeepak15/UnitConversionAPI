using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Dtos;
using UnitConversion.Core.Abstractions;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class CatalogController(IUnitRepository units) : ControllerBase
{
    /// <summary>Lists the supported conversion categories.</summary>
    [HttpGet("categories")]
    public ActionResult<IEnumerable<CategoryResponse>> GetCategories() =>
        Ok(units.GetCategories().Select(CategoryResponse.Map));

    /// <summary>Lists supported units, optionally filtered by category id.</summary>
    [HttpGet("units")]
    public ActionResult<IEnumerable<UnitResponse>> GetUnits([FromQuery] string? category = null) =>
        Ok(units.GetUnits(category).Select(UnitResponse.Map));
}
