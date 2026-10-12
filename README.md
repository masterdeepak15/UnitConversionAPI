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
