using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Core.Exceptions;

namespace UnitConversion.Api.ErrorHandling;

/// <summary>Translates domain exceptions into RFC 7807 problem responses; keeps controllers free of try/catch.</summary>
public sealed class ConversionExceptionHandler(IProblemDetailsService problems) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not ConversionException conversionException)
        {
            return false;
        }

        var (status, title) = conversionException switch
        {
            UnitNotFoundException => (StatusCodes.Status404NotFound, "Unit not found"),
            IncompatibleUnitsException => (StatusCodes.Status400BadRequest, "Incompatible units"),
            _ => (StatusCodes.Status400BadRequest, "Invalid conversion request"),
        };

        context.Response.StatusCode = status;
        return await problems.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails = new ProblemDetails { Status = status, Title = title, Detail = exception.Message },
        });
    }
}
