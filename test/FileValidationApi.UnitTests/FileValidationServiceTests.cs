using FileValidationApi.BusinessLogic.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FileValidationApi.UnitTests;

public class FileValidationServiceTests
{
    private readonly FileValidationService _fileValidationService;

    public FileValidationServiceTests()
    {
        var mockLogger = Substitute.For<ILogger<FileValidationService>>();

        _fileValidationService = new FileValidationService(mockLogger);
    }

    [Theory]
    [InlineData("Thomas 3299992", "Richard 3293982", "Rose 3293982")]
    [InlineData(" ", "\t", "\n")] // File consists of empty lines
    [InlineData()] // File does not contain lines
    public void ValidateFileContent_WhenValidFileProvided_ShouldReturnValidResult(params string[] fileLines)
    {
        var result = _fileValidationService.ValidateFileContent([.. fileLines]);

        result.FileValid.Should().BeTrue();
        result.InvalidLines.Should().BeEmpty();
    }

    [Theory]
    [InlineData("thomas 3299992", "Account name - not valid for 1 line 'thomas 3299992'")]
    // Invalid name (lowercase first letter)

    [InlineData("Thomas1 3299992", "Account name - not valid for 1 line 'Thomas1 3299992'")]
    // Invalid name (number in name)

    [InlineData("Thomas 2299992", "Account number - not valid for 1 line 'Thomas 2299992'")]
    // Invalid number (does not start with 3 or 4)

    [InlineData("Thomas 32999921", "Account number - not valid for 1 line 'Thomas 32999921'")]
    // Invalid number (contains more than 7 numbers)

    [InlineData("Thomas 329999", "Account number - not valid for 1 line 'Thomas 329999'")]
    // Invalid number (contains less than 7 numbers)

    [InlineData("Thomas1 2299992", "Account name, account number - not valid for 1 line 'Thomas1 2299992'")]
    // Invalid name (contains number), invalid number (does not start with 3 or 4).

    [InlineData("thomas 32999921", "Account name, account number - not valid for 1 line 'thomas 32999921'")]
    // Invalid name (lowercase first letter), invalid number (contains more than 7 numbers).

    [InlineData("Thomas32999921", "Format error for 1 line: 'Thomas32999921'")]
    // Format error (missing space between name and number)
    public void ValidateFileContent_WhenInvalidContentIsProvided_ShouldReturnValidResult(string line, string expectedInvalidLine)
    {
        var fileLines = new List<string>
        {
            line
        };

        var result = _fileValidationService.ValidateFileContent(fileLines);

        result.FileValid.Should().BeFalse();
        result.InvalidLines.Should().ContainSingle();
        result.InvalidLines[0].Should().Be(expectedInvalidLine);
    }

    [Fact]
    public void ValidateFileContent_WhenMultipleLinesProvided_ShouldReturnValidResult()
    {
        var fileLines = new List<string>
        {
            "Thomas 32999921", // Invalid number
            "Richard 3293982", // Valid
            "XAEA-12 8293982", // Invalid name and number
            "Rose 329a982",    // Invalid number
            "Bob 329398.",     // Invalid number
            "michael 3113902", // Invalid name
            "Rob 3113902p"     // Valid
        };

        var result = _fileValidationService.ValidateFileContent(fileLines);

        var expectedInvalidLines = new List<string>
        {
            "Account number - not valid for 1 line 'Thomas 32999921'",
            "Account name, account number - not valid for 3 line 'XAEA-12 8293982'",
            "Account number - not valid for 4 line 'Rose 329a982'",
            "Account number - not valid for 5 line 'Bob 329398.'",
            "Account name - not valid for 6 line 'michael 3113902'"
        };

        result.FileValid.Should().BeFalse();
        result.InvalidLines.Count.Should().Be(5);
        result.InvalidLines.Should().Equal(expectedInvalidLines);
    }
}