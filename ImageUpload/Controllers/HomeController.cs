using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImageUpload.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ImageUpload.App_start;
using ImageUpload.Utilies;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;

namespace ImageUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly object _defaultFormOptions = 6000000000000;
        private object _fileSizeLimit;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //[HttpPost("audio", Name = "UploadAudio")]
        //[DisableFormValueModelBinding]
        //[GenerateAntiforgeryTokenCookie]
        //[RequestSizeLimit(6000000000)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 6000000000)]
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 6000000000)]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
          
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    var filePath = Path.Combine("wwwroot/images", formFile.FileName);
                    // If file with same name exists delete it
                    // If file with same name exists delete it
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            //Task.Delay(2000).Wait();
            watch.Stop();
           // ViewData["time"] = "Execution Time: "+watch.ElapsedMilliseconds;
            ViewBag.Time = "Execution Time: " + watch.ElapsedMilliseconds;
            return View("Index");
        }

        /*public async Task<IActionResult> Upload(IFormFile[] files, [FromServices] IHostingEnvironment env)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            foreach (var file in files)
            {
                var filePath = Path.Combine("wwwroot/images", file.FileName);
                // Extract file name from whatever was posted by browser
                var fileName = System.IO.Path.GetFileName(filePath);

                // If file with same name exists delete it
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Create new local file and copy contents of uploaded file
                using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = file.OpenReadStream())
                {
                    await uploadedFile.CopyToAsync(localFile);
                    //uploadedFile.CopyTo(localFile);
                }
            }
            //Task.Delay(2000).Wait();
            watch.Stop();
            // ViewData["time"] = "Execution Time: "+watch.ElapsedMilliseconds;
            ViewBag.Time = "Execution Time: " + watch.ElapsedMilliseconds;
            //Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            //ViewData["message"] = $"{file.Length} bytes uploaded successfully!";
            return View("Index");
        }*/

        /*[HttpPost]
        [DisableFormValueModelBinding]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhysical()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");
                // Log error

                return BadRequest(ModelState);
            }

           
            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                70);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File",
                            $"The request couldn't be processed (Error 2).");
                        // Log error

                        return BadRequest(ModelState);
                    }
                    else
                    {
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();

                        // **WARNING!**
                        // In the following example, the file is saved without
                        // scanning the file's contents. In most production
                        // scenarios, an anti-virus/anti-malware scanner API
                        // is used on the file before making the file available
                        // for download or for use by other systems. 
                        // For more information, see the topic that accompanies 
                        // this sample.

                        object FileHelpers = null;
                        object _fileSizeLimit1 = _fileSizeLimit;
                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, ModelState,
                            _permittedExtensions, _fileSizeLimit1);

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        using (var targetStream = System.IO.File.Create(
                            Path.Combine(_targetFilePath, trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);

                            _logger.LogInformation(
                                "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
                                "'{TargetFilePath}' as {TrustedFileNameForFileStorage}",
                                trustedFileNameForDisplay, _targetFilePath,
                                trustedFileNameForFileStorage);
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return Created(nameof(StreamingController), null);
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
