namespace UnitConversion.Core.Domain;

/// <summary>A unit of measurement and how it relates to its category's base unit.</summary>
public sealed class MeasurementUnit
{
    private readonly string[] _aliases;

    public MeasurementUnit(
        string id,
        string name,
        string symbol,
        UnitCategory category,
        LinearScale scale,
        IEnumerable<string>? aliases = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(symbol);
        ArgumentNullException.ThrowIfNull(category);
        if (scale.Factor == 0m)
        {
            throw new ArgumentException("Scale factor must not be zero.", nameof(scale));
        }

        Id = id;
        Name = name;
        Symbol = symbol;
        Category = category;
        Scale = scale;
        _aliases = aliases?.ToArray() ?? [];
    }

    public string Id { get; }
    public string Name { get; }
    public string Symbol { get; }
    public UnitCategory Category { get; }
    public LinearScale Scale { get; }
    public IReadOnlyList<string> Aliases => _aliases;

    /// <summary>Every token a caller may use to identify this unit.</summary>
    public IEnumerable<string> Identifiers => _aliases.Prepend(Symbol).Prepend(Name).Prepend(Id);

    public decimal ToBase(decimal value) => Scale.ToBase(value);

    public decimal FromBase(decimal baseValue) => Scale.FromBase(baseValue);
}
