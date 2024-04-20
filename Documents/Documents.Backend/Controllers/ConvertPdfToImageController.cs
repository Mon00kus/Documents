using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.PdfToImageConverter;
using System.Net.Mime;

using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace Documents.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvertPdfToImageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConvertPdfToImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("ConvertPdfToImage")]
        public async Task<IActionResult> ConvertPdfToImage(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return BadRequest("Archivo no válido.");
            }

            var currentDirectory = _configuration["W0rkingPath:Current"];
            var targetFileName = Path.ChangeExtension(pdfFile.FileName, ".png");
            var targetPath = Path.Combine(currentDirectory!, targetFileName);
            var sourceFilePath = Path.Combine(currentDirectory!, pdfFile.FileName);

            try
            {
                await using (var inputStream = new FileStream(sourceFilePath, FileMode.Create, FileAccess.Write))
                {
                    await pdfFile.CopyToAsync(inputStream);
                }

                await using (var inputStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
                {
                    PdfToImageConverter imageConverter = new PdfToImageConverter();
                    imageConverter.Load(inputStream);

                    // Convierte el PDF a imagen
                    Stream outputStream = imageConverter.Convert(0, false, false);

                    await using (var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
                    {
                        outputStream.Seek(0, SeekOrigin.Begin);
                        await outputStream.CopyToAsync(fileStream);
                    }

                    //////////
                    // Procesamiento de imagen para aclarar usando Emgu CV

                    //Mat image = CvInvoke.Imread(targetPath, ImreadModes.Color);

                    //Mat destination = new Mat();

                    //CvInvoke.AddWeighted(image, 1.5, image, 0, 50, destination);

                    //destination.Save(targetPath);

                    Mat image = new Mat(targetPath, ImreadModes.Color);

                    // Aplicar la nitidez
                    Mat sharpenedImage = new Mat();
                    Sharpen(image, sharpenedImage);

                    // Guardar la imagen procesada
                    sharpenedImage.Save(targetPath);
                    /////////
                    
                    await using (var memoryStream = new MemoryStream())
                    {
                        outputStream.Seek(0, SeekOrigin.Begin);
                        await outputStream.CopyToAsync(memoryStream);
                        return File(memoryStream.ToArray(), MediaTypeNames.Image.Png, targetFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al convertir el archivo PDF -- " + ex.Message);
            }
            finally
            {
                if (System.IO.File.Exists(sourceFilePath))
                {
                    System.IO.File.Delete(sourceFilePath);
                }
            }
        }

        private void Sharpen(Mat image, Mat output)
        {
            float[,] kernelValues = {
                { -1, -1, -1 },
                { -1,  9, -1 },
                { -1, -1, -1 }
            };
            Matrix<float> kernel = new Matrix<float>(kernelValues);

            // Aplicar el filtro
            CvInvoke.Filter2D(image, output, kernel, new Point(-1, -1));            
        }
    }
}