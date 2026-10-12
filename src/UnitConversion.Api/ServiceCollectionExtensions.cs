using UnitConversion.Core.Abstractions;
using UnitConversion.Core.Catalog;
using UnitConversion.Core.Services;

namespace UnitConversion.Api;

public static class ServiceCollectionExtensions
{
    /// <summary>Composition root for the conversion domain. Register new categories here.</summary>
    public static IServiceCollection AddUnitConversion(this IServiceCollection services)
    {
        services.AddSingleton<IUnitDefinitionSource, LengthUnits>();
        services.AddSingleton<IUnitDefinitionSource, TemperatureUnits>();
        services.AddSingleton<IUnitDefinitionSource, MassUnits>();
        services.AddSingleton<IUnitRepository, InMemoryUnitRepository>();
        services.AddSingleton<IUnitConverter, UnitConverter>();
        return services;
    }
}
