using UnitConversion.Core.Abstractions;
using UnitConversion.Core.Domain;

namespace UnitConversion.Core.Catalog;

/// <summary>
/// Hardcoded catalog assembled from <see cref="IUnitDefinitionSource"/>s and indexed once at startup.
/// Identifiers must be globally unique so a lookup never needs a category hint.
/// </summary>
public sealed class InMemoryUnitRepository : IUnitRepository
{
    private readonly List<UnitCategory> _categories = [];
    private readonly List<MeasurementUnit> _units = [];
    private readonly Dictionary<string, MeasurementUnit> _index = new(StringComparer.OrdinalIgnoreCase);

    public InMemoryUnitRepository(IEnumerable<IUnitDefinitionSource> sources)
    {
        foreach (var source in sources)
        {
            _categories.Add(source.Category);
            foreach (var unit in source.GetUnits())
            {
                _units.Add(unit);
                Index(unit);
            }
        }
    }

    public IReadOnlyCollection<UnitCategory> GetCategories() => _categories;

    public IReadOnlyCollection<MeasurementUnit> GetUnits(string? categoryId = null) =>
        string.IsNullOrWhiteSpace(categoryId)
            ? _units
            : _units.Where(u => string.Equals(u.Category.Id, categoryId, StringComparison.OrdinalIgnoreCase)).ToList();

    public MeasurementUnit? Find(string identifier) =>
        _index.GetValueOrDefault(identifier.Trim());

    private void Index(MeasurementUnit unit)
    {
        foreach (var identifier in unit.Identifiers.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!_index.TryAdd(identifier, unit))
            {
                throw new InvalidOperationException(
                    $"Identifier '{identifier}' of unit '{unit.Id}' is already used by '{_index[identifier].Id}'.");
            }
        }
    }
}
