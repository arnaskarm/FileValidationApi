namespace FileValidationApi.BusinessLogic.Services;

/// <inheritdoc cref="IFileProcessingService"/>
public class FileProcessingService : IFileProcessingService
{
    private const string SupportedFileType = ".txt";

    /// <inheritdoc/>
    public async Task<List<string>> ReadFileLines(IFormFile file)
    {
        ValidateFile(file);

        using var reader = new StreamReader(file.OpenReadStream());

        var content = await reader.ReadToEndAsync();

        return [.. content.Split('\n', StringSplitOptions.RemoveEmptyEntries)];
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new InvalidDataException("The uploaded file is empty or missing");
        }

        string fileType = Path.GetExtension(file.FileName);

        if (!fileType.Equals(SupportedFileType, StringComparison.OrdinalIgnoreCase))
        {
            throw new NotSupportedException($"Unsupported file type: {fileType}. Expected: {SupportedFileType}");
        }
    }
}
