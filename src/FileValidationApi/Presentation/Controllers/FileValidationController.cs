using FileValidationApi.BusinessLogic.Models;
using FileValidationApi.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileValidationApi.Presentation.Controllers;

/// <summary>
/// Controller responsible for handling file upload and validation requests.
/// </summary>
[Route("api")]
[ApiController]
public class FileValidationController(IFileProcessingService fileProcessingService,
    IFileValidationService fileValidationService, ILogger<FileValidationController> logger) : ControllerBase
{
    /// <summary>
    /// Uploads and validates the content of an uploaded file.
    /// </summary>
    /// <param name="file">The ".txt" format file to be uploaded and validated.</param>
    /// <returns>A result indicating the validation status of the uploaded file.</returns>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(FileValidationResult), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(415)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ValidateFile(IFormFile file)
    {
        try
        {
            List<string> fileLines = await fileProcessingService.ReadFileLines(file);

            var validationResult = fileValidationService.ValidateFileContent(fileLines);

            return Ok(validationResult);
        }
        catch (InvalidDataException ex)
        {
            logger.LogWarning(ex, "File data validation failed.");
            return BadRequest("File is not provided or empty.");
        }
        catch (NotSupportedException ex)
        {
            logger.LogWarning(ex, "Unsupported fyle type.");
            return StatusCode(StatusCodes.Status415UnsupportedMediaType, "The file type is not supported.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the request.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
        }
    }
}
