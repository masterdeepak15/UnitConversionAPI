using UnitConversion.Core.Catalog;
using UnitConversion.Core.Exceptions;
using UnitConversion.Core.Services;

namespace UnitConversion.Tests;

public class UnitConverterTests
{
    private readonly UnitConverter _converter = new(new InMemoryUnitRepository(
        [new LengthUnits(), new TemperatureUnits(), new MassUnits()]));

    [Theory]
    [InlineData(1, "km", "m", 1000)]
    [InlineData(1, "mile", "km", 1.609344)]
    [InlineData(12, "inches", "ft", 1)]
    [InlineData(100, "celsius", "fahrenheit", 212)]
    [InlineData(32, "F", "C", 0)]
    [InlineData(0, "C", "K", 273.15)]
    [InlineData(-40, "C", "F", -40)]
    [InlineData(1, "kg", "lb", 2.2046226218)]
    [InlineData(16, "oz", "lb", 1)]
    [InlineData(5, "g", "g", 5)]
    public void Convert_ReturnsExpectedResult(decimal value, string from, string to, decimal expected)
    {
        var result = _converter.Convert(value, from, to);

        Assert.Equal(expected, result.Result);
    }

    [Fact]
    public void Convert_IsCaseInsensitive()
    {
        Assert.Equal(1000m, _converter.Convert(1, "KM", "Meters").Result);
    }

    [Fact]
    public void Convert_UnknownUnit_Throws()
    {
        Assert.Throws<UnitNotFoundException>(() => _converter.Convert(1, "parsec", "m"));
    }

    [Fact]
    public void Convert_BlankUnit_Throws()
    {
        Assert.Throws<UnitNotFoundException>(() => _converter.Convert(1, " ", "m"));
    }

    [Fact]
    public void Convert_DifferentCategories_Throws()
    {
        Assert.Throws<IncompatibleUnitsException>(() => _converter.Convert(1, "kg", "m"));
    }

    [Fact]
    public void Convert_Overflow_ThrowsValueOutOfRange()
    {
        Assert.Throws<ValueOutOfRangeException>(() => _converter.Convert(decimal.MaxValue, "km", "mm"));
    }

    [Fact]
    public void Repository_DuplicateIdentifier_Throws()
    {
        Assert.Throws<InvalidOperationException>(() =>
            new InMemoryUnitRepository([new LengthUnits(), new LengthUnits()]));
    }
}
