using UnitConversion.Core.Abstractions;
using UnitConversion.Core.Domain;
using UnitConversion.Core.Exceptions;

namespace UnitConversion.Core.Services;

/// <summary>Converts between units by routing through the category's base unit.</summary>
public sealed class UnitConverter(IUnitRepository units) : IUnitConverter
{
    /// <summary>Decimal places kept in results; hides rounding noise such as 212.0000000001.</summary>
    public const int ResultPrecision = 10;

    public ConversionResult Convert(decimal value, string from, string to)
    {
        var source = Resolve(from);
        var target = Resolve(to);

        if (source.Category != target.Category)
        {
            throw new IncompatibleUnitsException(source.Id, target.Id, source.Category.Id, target.Category.Id);
        }

        try
        {
            var result = target.FromBase(source.ToBase(value));
            return new ConversionResult(value, source, target, Math.Round(result, ResultPrecision));
        }
        catch (OverflowException)
        {
            throw new ValueOutOfRangeException($"The value {value} is too large to convert.");
        }
    }

    private MeasurementUnit Resolve(string identifier) =>
        string.IsNullOrWhiteSpace(identifier)
            ? throw new UnitNotFoundException(identifier ?? string.Empty)
            : units.Find(identifier) ?? throw new UnitNotFoundException(identifier);
}
