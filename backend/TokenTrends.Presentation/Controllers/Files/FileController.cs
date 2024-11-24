using Microsoft.AspNetCore.Mvc;
using TokenTrends.Application.Services.FileServices;

namespace TokenTrends.Presentation.Controllers.Files;

[ApiController]
[Route("api/")]
public class FileController(IFileService fileService)
    : ControllerBase
{
    [HttpGet("file/{bucket}/{fileName}")]
    public async Task<ActionResult> GetFile(
        [FromRoute] string bucket,
        [FromRoute] string fileName,
        CancellationToken cancellationToken)
    {
        var downloadedFile = 
            await fileService.DownloadFileAsync(bucket, fileName, cancellationToken);

        return File(downloadedFile.Stream, downloadedFile.ContentType, downloadedFile.FileName);
    }
}