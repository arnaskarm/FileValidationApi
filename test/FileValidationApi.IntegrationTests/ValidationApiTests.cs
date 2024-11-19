using FileValidationApi.BusinessLogic.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FileValidationApi.IntegrationTests;

public class ValidationApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ApiUrl = "/api/validate";

    private readonly HttpClient _client = factory.CreateClient();
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    [Fact]
    public async Task UploadFile_WhenValidFileProvided_ShouldReturnFileValidTrueWithoutInvalidLines()
    {
        var fileContent = "Thomas 3299992\nRichard 3293982";

        var response = await UploadFile(fileContent);

        var expectedResult = new FileValidationResult();

        await AssertResponseIsValid(expectedResult, response);
    }

    [Fact]
    public async Task UploadFile_WhenInvalidFileProvided_ShouldReturnFileValidFalseWithInvalidLines()
    {
        var fileContent = "Thomas 32999921\nRose 329a982";

        var response = await UploadFile(fileContent);

        var expectedResult = new FileValidationResult()
        {
            InvalidLines =
            [
                "Account number - not valid for 1 line 'Thomas 32999921'",
                "Account number - not valid for 2 line 'Rose 329a982'"
            ]
        };

        await AssertResponseIsValid(expectedResult, response);
    }

    [Fact]
    public async Task UploadFile_WhenEmptyFileProvided_ShouldReturnBadRequest()
    {
        var fileContent = "";

        var response = await UploadFile(fileContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Contain("File is not provided or empty.");
    }

    [Fact]
    public async Task UploadFile_WhenFileWithInvalidExtensionProvided_ShouldReturnUnsupportedMediaType()
    {
        var fileContent = "Thomas 3293982\nMichael 4113902p";

        var file = CreateMultipartFileContent(fileContent, "file.invalid");

        var response = await _client.PostAsync(ApiUrl, file);

        response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Contain("The file type is not supported.");
    }

    private static MultipartFormDataContent CreateMultipartFileContent(string content, string fileName = "file.txt")
    {
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

        return new MultipartFormDataContent
        {
            { fileContent, "file", fileName }
        };
    }

    private async Task<HttpResponseMessage> UploadFile(string fileContent)
    {
        var file = CreateMultipartFileContent(fileContent);

        return await _client.PostAsync(ApiUrl, file);
    }

    private static async Task AssertResponseIsValid(FileValidationResult expectedResult, HttpResponseMessage actualResponse)
    {
        actualResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var actualResult = await GetDeserializedContent(actualResponse.Content);

        actualResult.FileValid.Should().Be(expectedResult.FileValid);
        actualResult.InvalidLines.Count.Should().Be(expectedResult.InvalidLines.Count);
        actualResult.InvalidLines.Should().Equal(expectedResult.InvalidLines);
    }

    private static async Task<FileValidationResult> GetDeserializedContent(HttpContent httpContent)
    {
        string content = await httpContent.ReadAsStringAsync();

        return JsonSerializer.Deserialize<FileValidationResult>(content, _jsonOptions)
            ?? throw new JsonException($"Failed to deserialize the response content: {content}");
    }
}