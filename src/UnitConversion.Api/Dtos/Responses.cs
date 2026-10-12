using UnitConversion.Core.Domain;

namespace UnitConversion.Api.Dtos;

public sealed record CategoryResponse(string Id, string Name)
{
    public static CategoryResponse Map(UnitCategory category) => new(category.Id, category.Name);
}

public sealed record UnitResponse(string Id, string Name, string Symbol, string Category, IReadOnlyList<string> Aliases)
{
    public static UnitResponse Map(MeasurementUnit unit) =>
        new(unit.Id, unit.Name, unit.Symbol, unit.Category.Id, unit.Aliases);
}

public sealed record ConversionResponse(
    string Category,
    decimal Value,
    string From,
    string To,
    decimal Result)
{
    public static ConversionResponse Map(ConversionResult result) =>
        new(result.From.Category.Id, result.Value, result.From.Id, result.To.Id, result.Result);
}
