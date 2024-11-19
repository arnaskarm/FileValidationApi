namespace FileValidationApi.BusinessLogic.Models;

/// <summary>
/// Represents the duration of a validation process for a specific line.
/// </summary>
public class LineValidationDuration(int lineNumber, string line, long durationInMilliseconds)
{
    /// <summary>
    /// The line number in the content which is validated.
    /// </summary>
    public int LineNumber { get; } = lineNumber;

    /// <summary>
    /// The actual line content that was validated.
    /// </summary>
    public string Line { get; } = line;

    /// <summary>
    /// The duration in milliseconds for how long the validation took.
    /// </summary>
    public long DurationInMilliseconds { get; } = durationInMilliseconds;
}
