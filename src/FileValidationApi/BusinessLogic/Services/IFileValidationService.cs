using FileValidationApi.BusinessLogic.Models;

namespace FileValidationApi.BusinessLogic.Services;

/// <summary>
/// Service for file validation.
/// </summary>
public interface IFileValidationService
{
    /// <summary>
    /// Validates file content.
    /// </summary>
    public FileValidationResult ValidateFileContent(List<string> fileLines);
}
