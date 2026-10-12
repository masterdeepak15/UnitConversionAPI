namespace UnitConversion.Core.Domain;

/// <summary>A family of mutually convertible units (e.g. length, temperature, mass).</summary>
public sealed record UnitCategory(string Id, string Name);
