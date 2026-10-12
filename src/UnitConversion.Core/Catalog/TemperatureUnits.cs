using UnitConversion.Core.Abstractions;
using UnitConversion.Core.Domain;

namespace UnitConversion.Core.Catalog;

/// <summary>Temperature scales. Base unit: kelvin.</summary>
public sealed class TemperatureUnits : IUnitDefinitionSource
{
    private const decimal FiveNinths = 5m / 9m;

    public UnitCategory Category { get; } = new("temperature", "Temperature");

    public IEnumerable<MeasurementUnit> GetUnits()
    {
        yield return new("kelvin", "Kelvin", "K", Category, new LinearScale(1m), ["kelvins"]);
        yield return new("celsius", "Celsius", "C", Category, new LinearScale(1m, 273.15m), ["degC", "centigrade"]);
        yield return new("fahrenheit", "Fahrenheit", "F", Category,
            new LinearScale(FiveNinths, 459.67m * FiveNinths), ["degF"]);
        yield return new("rankine", "Rankine", "R", Category, new LinearScale(FiveNinths), ["degR"]);
    }
}
