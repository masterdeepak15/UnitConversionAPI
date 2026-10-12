namespace UnitConversion.Api;

public static class SwaggerUiExtensions
{
    private const string Html = """
        <!doctype html>
        <html lang="en">
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>Unit Conversion API - Swagger UI</title>
          <link rel="stylesheet" href="https://unpkg.com/swagger-ui-dist@5.17.14/swagger-ui.css" />
        </head>
        <body>
          <div id="swagger-ui"></div>
          <script src="https://unpkg.com/swagger-ui-dist@5.17.14/swagger-ui-bundle.js"></script>
          <script>
            window.ui = SwaggerUIBundle({ url: "/openapi/v1.json", dom_id: "#swagger-ui" });
          </script>
        </body>
        </html>
        """;

    /// <summary>Serves Swagger UI at /swagger, rendering the built-in OpenAPI document (/openapi/v1.json).</summary>
    public static IEndpointConventionBuilder MapSwaggerUi(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/swagger", () => Results.Content(Html, "text/html")).ExcludeFromDescription();
}
