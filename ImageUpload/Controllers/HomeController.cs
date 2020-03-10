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

namespace ImageUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
