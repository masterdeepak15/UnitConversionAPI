using UnitConversion.Core.Domain;

namespace UnitConversion.Core.Abstractions;

public interface IUnitConverter
{
    ConversionResult Convert(decimal value, string from, string to);
}
