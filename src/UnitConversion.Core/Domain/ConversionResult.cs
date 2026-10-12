namespace UnitConversion.Core.Domain;

public sealed record ConversionResult(
    decimal Value,
    MeasurementUnit From,
    MeasurementUnit To,
    decimal Result);
