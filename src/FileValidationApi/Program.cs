using FileValidationApi.BusinessLogic.Services;
using FileValidationApi.Presentation.Extensions;

namespace FileValidationApi;

/// <summary>
/// The entry point of the application.
/// </summary>
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.ConfigureSwagger();
        builder.Services.AddSingleton<IFileProcessingService, FileProcessingService>();
        builder.Services.AddSingleton<IFileValidationService, FileValidationService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
