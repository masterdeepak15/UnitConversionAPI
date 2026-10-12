using UnitConversion.Core.Abstractions;
using UnitConversion.Core.Domain;

namespace UnitConversion.Core.Catalog;

/// <summary>Length units. Base unit: metre.</summary>
public sealed class LengthUnits : IUnitDefinitionSource
{
    public UnitCategory Category { get; } = new("length", "Length");

    public IEnumerable<MeasurementUnit> GetUnits()
    {
        yield return Unit("meter", "Meter", "m", 1m, "metre", "meters", "metres");
        yield return Unit("kilometer", "Kilometer", "km", 1000m, "kilometre", "kilometers", "kilometres");
        yield return Unit("centimeter", "Centimeter", "cm", 0.01m, "centimetre", "centimeters", "centimetres");
        yield return Unit("millimeter", "Millimeter", "mm", 0.001m, "millimetre", "millimeters", "millimetres");
        yield return Unit("mile", "Mile", "mi", 1609.344m, "miles");
        yield return Unit("yard", "Yard", "yd", 0.9144m, "yards");
        yield return Unit("foot", "Foot", "ft", 0.3048m, "feet");
        yield return Unit("inch", "Inch", "in", 0.0254m, "inches");
        yield return Unit("nautical-mile", "Nautical mile", "nmi", 1852m, "nautical-miles");
    }

    private MeasurementUnit Unit(string id, string name, string symbol, decimal metres, params string[] aliases) =>
        new(id, name, symbol, Category, new LinearScale(metres), aliases);
}
