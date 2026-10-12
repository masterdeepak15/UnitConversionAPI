namespace UnitConversion.Core.Exceptions;

/// <summary>Base type for expected, caller-correctable conversion failures.</summary>
public abstract class ConversionException(string message) : Exception(message);

public sealed class UnitNotFoundException(string identifier)
    : ConversionException($"Unknown unit '{identifier}'.")
{
    public string Identifier { get; } = identifier;
}

public sealed class IncompatibleUnitsException(string from, string to, string fromCategory, string toCategory)
    : ConversionException($"Cannot convert '{from}' ({fromCategory}) to '{to}' ({toCategory}): units belong to different categories.");

public sealed class ValueOutOfRangeException(string message) : ConversionException(message);
