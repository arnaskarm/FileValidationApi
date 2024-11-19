namespace FileValidationApi.BusinessLogic.Models;

/// <summary>
/// Represents the result of a file validation process.
/// </summary>
public class FileValidationResult
{
    /// <summary>
    /// Indicates whether the file is valid. It returns true if there are no invalid lines.
    /// </summary>
    public bool FileValid => InvalidLines.Count == 0;

    /// <summary>
    /// A list of strings representing the lines in the file that are invalid.
    /// </summary>
    public List<string> InvalidLines { get; set; } = [];
}
