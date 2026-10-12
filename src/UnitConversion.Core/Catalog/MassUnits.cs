using UnitConversion.Core.Abstractions;
using UnitConversion.Core.Domain;

namespace UnitConversion.Core.Catalog;

/// <summary>Weight/mass units. Base unit: kilogram.</summary>
public sealed class MassUnits : IUnitDefinitionSource
{
    public UnitCategory Category { get; } = new("mass", "Weight / Mass");

    public IEnumerable<MeasurementUnit> GetUnits()
    {
        yield return Unit("kilogram", "Kilogram", "kg", 1m, "kilograms", "kilo", "kilos");
        yield return Unit("gram", "Gram", "g", 0.001m, "grams");
        yield return Unit("milligram", "Milligram", "mg", 0.000001m, "milligrams");
        yield return Unit("tonne", "Metric tonne", "t", 1000m, "tonnes", "metric-ton", "metric-tons");
        yield return Unit("pound", "Pound", "lb", 0.45359237m, "pounds", "lbs");
        yield return Unit("ounce", "Ounce", "oz", 0.028349523125m, "ounces");
        yield return Unit("stone", "Stone", "st", 6.35029318m, "stones");
    }

    private MeasurementUnit Unit(string id, string name, string symbol, decimal kilograms, params string[] aliases) =>
        new(id, name, symbol, Category, new LinearScale(kilograms), aliases);
}
