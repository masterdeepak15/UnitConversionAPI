# Unit Conversion API

An ASP.NET Core (.NET 10) Web API that converts numeric values between units of measurement.
Supported categories: **length**, **temperature** and **weight/mass**.

## Run locally

Prerequisite: [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project src/UnitConversion.Api
```

The launch profile prints the listening URL (`http://localhost:5022`). In Development, Swagger UI is at
`/swagger` and the OpenAPI document at `/openapi/v1.json`. Swagger UI loads its assets from a CDN (unpkg), so the
page needs internet access; the API itself does not.

```bash
dotnet test        # unit + integration tests
```

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/conversions?value=100&from=celsius&to=fahrenheit` | Convert a value |
| GET | `/api/categories` | List categories |
| GET | `/api/units?category=length` | List units (filter optional) |

Units can be given by id, name, symbol or alias, case-insensitive (`km`, `Kilometers`, `mile`, `lbs`, `F`).

```json
{ "category": "temperature", "value": 100, "from": "celsius", "to": "fahrenheit", "result": 212 }
```

Errors use RFC 7807 problem details: `404` for an unknown unit, `400` for units from different
categories, a malformed/missing parameter, or a value too large to convert.

## Supported conversions

Conversion works between **any two units in the same category** (e.g. every length unit to every other length unit),
which is 9 x 8 = 72 length, 7 x 6 = 42 mass and 4 x 3 = 12 temperature conversions, plus identity conversions.
Converting across categories (e.g. `kg` to `m`) is rejected with `400`.
The live list is always available from `GET /api/units`.

Lookups are case-insensitive and accept the **id**, **name**, **symbol** or any **alias** below.

### Length (base unit: meter)

| Id | Name | Symbol | Aliases | Value in meters |
|----|------|--------|---------|-----------------|
| `meter` | Meter | `m` | metre, meters, metres | 1 |
| `kilometer` | Kilometer | `km` | kilometre, kilometers, kilometres | 1000 |
| `centimeter` | Centimeter | `cm` | centimetre, centimeters, centimetres | 0.01 |
| `millimeter` | Millimeter | `mm` | millimetre, millimeters, millimetres | 0.001 |
| `mile` | Mile | `mi` | miles | 1609.344 |
| `yard` | Yard | `yd` | yards | 0.9144 |
| `foot` | Foot | `ft` | feet | 0.3048 |
| `inch` | Inch | `in` | inches | 0.0254 |
| `nautical-mile` | Nautical mile | `nmi` | nautical-miles | 1852 |

### Weight / Mass (base unit: kilogram)

| Id | Name | Symbol | Aliases | Value in kilograms |
|----|------|--------|---------|--------------------|
| `kilogram` | Kilogram | `kg` | kilograms, kilo, kilos | 1 |
| `gram` | Gram | `g` | grams | 0.001 |
| `milligram` | Milligram | `mg` | milligrams | 0.000001 |
| `tonne` | Metric tonne | `t` | tonnes, metric-ton, metric-tons | 1000 |
| `pound` | Pound | `lb` | pounds, lbs | 0.45359237 |
| `ounce` | Ounce | `oz` | ounces | 0.028349523125 |
| `stone` | Stone | `st` | stones | 6.35029318 |

### Temperature (base unit: kelvin)

| Id | Name | Symbol | Aliases | Relation to kelvin (K) |
|----|------|--------|---------|------------------------|
| `kelvin` | Kelvin | `K` | kelvins | K = K |
| `celsius` | Celsius | `C` | degC, centigrade | K = C + 273.15 |
| `fahrenheit` | Fahrenheit | `F` | degF | K = (F + 459.67) x 5/9 |
| `rankine` | Rankine | `R` | degR | K = R x 5/9 |

### Example calls

| Request | Result |
|---------|--------|
| `/api/conversions?value=1&from=km&to=m` | 1000 |
| `/api/conversions?value=1&from=mile&to=km` | 1.609344 |
| `/api/conversions?value=12&from=inches&to=ft` | 1 |
| `/api/conversions?value=100&from=celsius&to=fahrenheit` | 212 |
| `/api/conversions?value=-40&from=C&to=F` | -40 |
| `/api/conversions?value=0&from=C&to=K` | 273.15 |
| `/api/conversions?value=1&from=kg&to=lb` | 2.2046226218 |
| `/api/conversions?value=16&from=oz&to=lb` | 1 |
| `/api/conversions?value=1&from=kg&to=m` | `400` incompatible units |
| `/api/conversions?value=1&from=parsec&to=m` | `404` unknown unit |

## Structure

```
src/UnitConversion.Core   Domain + conversion logic, no web or framework dependencies
src/UnitConversion.Api    HTTP layer: controllers, DTOs, error handling, composition root
tests/UnitConversion.Tests  Unit tests (converter) and integration tests (HTTP)
```

## Design decisions and trade-offs

- **One formula for every category.** Each unit stores `base = value * factor + offset` against its
  category's base unit (metre, kilogram, kelvin). Conversion is `from -> base -> to`, so N units need
  N definitions rather than N² pairwise rules, and temperature (which needs an offset) needs no special case.
- **Open/closed catalog.** Each category is an `IUnitDefinitionSource`. Adding a category (e.g. volume) is a new
  class plus one registration line in `ServiceCollectionExtensions`; no existing code changes.
- **Dependency inversion.** The converter depends on `IUnitRepository`, not on the hardcoded data. Hundreds of units
  stored in a database later means a new repository implementation only. The in-memory index gives O(1) lookup.
- **Single responsibility.** Catalog data, lookup, conversion math, HTTP mapping and error translation each live in
  their own type. Domain failures are exceptions translated to HTTP in one place (`ConversionExceptionHandler`).
- **`decimal` over `double`.** Avoids binary floating-point noise (`100 °C` is exactly `212 °F`). Results are rounded to
  10 decimal places. The cost is a smaller range and slower arithmetic, which is fine here.
- **Unit identifiers are globally unique,** so callers don't need to pass a category. The repository fails fast at
  startup if two units collide.
- **Not handled (yet):** temperatures below absolute zero are not rejected; there is no auth, caching or pagination.
