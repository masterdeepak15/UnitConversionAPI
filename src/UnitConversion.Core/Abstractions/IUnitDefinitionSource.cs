using UnitConversion.Core.Domain;

namespace UnitConversion.Core.Abstractions;

/// <summary>
/// Contributes one category and its units to the catalog. Adding a new category means adding a
/// new source and registering it - no existing code changes (open/closed).
/// </summary>
public interface IUnitDefinitionSource
{
    UnitCategory Category { get; }

    IEnumerable<MeasurementUnit> GetUnits();
}
