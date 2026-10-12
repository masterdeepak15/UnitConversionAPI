using UnitConversion.Core.Domain;

namespace UnitConversion.Core.Abstractions;

/// <summary>Read-only access to the unit catalog. Swap the implementation to move to a database.</summary>
public interface IUnitRepository
{
    IReadOnlyCollection<UnitCategory> GetCategories();

    IReadOnlyCollection<MeasurementUnit> GetUnits(string? categoryId = null);

    /// <summary>Finds a unit by id, name, symbol or alias (case-insensitive); null when unknown.</summary>
    MeasurementUnit? Find(string identifier);
}
