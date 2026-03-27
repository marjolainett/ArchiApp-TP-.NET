using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MarjoNamespace
{
    public static class ResizeHttpTrigger
    {
        [FunctionName("ResizeHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("ResizeHttpTrigger processed a request.");

            string wStr = req.Query["w"];
            string hStr = req.Query["h"];

            if (!int.TryParse(wStr, out int w) || !int.TryParse(hStr, out int h) || w <= 0 || h <= 0)
                return new BadRequestObjectResult("Please provide positive integers for 'w' and 'h' in the query string.");

            byte[] targetImageBytes;

            try
            {
                using (var msInput = new MemoryStream())
                {
                    await req.Body.CopyToAsync(msInput);
                    msInput.Position = 0;

                    using (var image = Image.Load(msInput))
                    {
                        image.Mutate(x => x.Resize(w, h));
                        using (var msOutput = new MemoryStream())
                        {
                            image.SaveAsJpeg(msOutput);
                            targetImageBytes = msOutput.ToArray();
                        }
                    }
                }
            }
            catch (UnknownImageFormatException)
            {
                return new BadRequestObjectResult("The request body must be a valid image.");
            }

            return new FileContentResult(targetImageBytes, "image/jpeg");
        }
    }
}