using FileValidationApi.Presentation.Controllers;
using Microsoft.OpenApi.Models;

namespace FileValidationApi.Presentation.Extensions;

/// <summary>
/// Service collection extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures Swagger.
    /// </summary>
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "File Validation API",
                Version = "v1"
            });

            string? xmlFilename = $"{typeof(FileValidationController).Assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}
