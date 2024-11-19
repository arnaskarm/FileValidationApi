namespace FileValidationApi.BusinessLogic.Services;

/// <summary>
/// Defines a service for processing files.
/// </summary>
public interface IFileProcessingService
{
    /// <summary>
    /// Reads a file and returns its lines.
    /// </summary>
    public Task<List<string>> ReadFileLines(IFormFile file);
}