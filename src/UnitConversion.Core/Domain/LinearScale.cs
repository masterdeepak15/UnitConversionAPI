namespace UnitConversion.Core.Domain;

/// <summary>
/// Affine relationship between a unit and its category's base unit:
/// <c>base = value * Factor + Offset</c>. Plain ratio units (length, mass) use a zero offset;
/// temperature scales use a non-zero offset. One formula covers every category.
/// </summary>
public readonly record struct LinearScale(decimal Factor, decimal Offset = 0m)
{
    public decimal ToBase(decimal value) => value * Factor + Offset;

    public decimal FromBase(decimal baseValue) => (baseValue - Offset) / Factor;
}
