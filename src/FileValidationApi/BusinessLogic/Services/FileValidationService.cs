using FileValidationApi.BusinessLogic.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FileValidationApi.BusinessLogic.Services;

/// <inheritdoc cref="IFileValidationService"/>
public partial class FileValidationService(ILogger<FileValidationService> logger) : IFileValidationService
{
    private const string NamePattern = @"^[A-Z][a-zA-Z]*$";
    private const string NumberPattern = @"^(3|4)\d{6}(p?)$";
    private const string AccountName = "Account name";
    private const string AccountNumber = "Account number";

    /// <inheritdoc/>
    public FileValidationResult ValidateFileContent(List<string> fileLines)
    {
        var validationResult = new FileValidationResult();
        var validationDurations = new List<LineValidationDuration>();

        for (int i = 0; i < fileLines.Count; i++)
        {
            string line = fileLines[i].Trim();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var stopWatch = Stopwatch.StartNew();

            var lineParts = line.Split(' ');

            if (lineParts.Length == 2)
            {
                var validationErrors = ValidateLine(lineParts);

                if (validationErrors.Count > 0)
                {
                    validationResult.InvalidLines.Add($"{string.Join(", ", validationErrors)} - not valid for {i + 1} line '{line}'");
                }
            }
            else
            {
                validationResult.InvalidLines.Add($"Format error for {i + 1} line: '{line}'");
            }

            stopWatch.Stop();
            validationDurations.Add(new LineValidationDuration(i + 1, line, stopWatch.ElapsedMilliseconds));
        }

        LogValidationDurations(validationDurations);

        return validationResult;
    }

    private void LogValidationDurations(List<LineValidationDuration> validationDurations)
    {
        foreach (var validationDuration in validationDurations)
        {
            logger.LogInformation("Line {LineNumber} validated in {DurationInMilliseconds}ms - {LineContent}", validationDuration.LineNumber, validationDuration.DurationInMilliseconds, validationDuration.Line);
        }
    }

    private static List<string> ValidateLine(string[] lineParts)
    {
        var (name, number) = (lineParts[0], lineParts[1]);

        var isValidName = NameRegex().IsMatch(name);
        var isValidNumber = NumberRegex().IsMatch(number);

        var errors = new List<string>();

        if (!isValidName)
        {
            errors.Add(AccountName);
        }

        if (!isValidNumber)
        {
            errors.Add(errors.Count == 0 ? AccountNumber : AccountNumber.ToLower());
        }

        return errors;
    }

    [GeneratedRegex(NamePattern)]
    private static partial Regex NameRegex();

    [GeneratedRegex(NumberPattern)]
    private static partial Regex NumberRegex();
}