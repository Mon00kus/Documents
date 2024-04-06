using Microsoft.AspNetCore.Mvc;
using Syncfusion.PdfToImageConverter;
using System.Net.Mime;

namespace Documents.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvertPdfToImageController : ControllerBase
    {
        [HttpPost]
        public Task<IActionResult> Post(IFormFile pdfFile)
        {
            FileStream inputStream = null!;

            if (pdfFile == null || pdfFile.Length == 0)
            {
                return Task.FromResult<IActionResult>(BadRequest("Archivo no válido."));
            }

            try
            { 
                PdfToImageConverter imageConverter = new PdfToImageConverter();

                // Carga el documento PDF como un stream
                
                inputStream = new FileStream($"c:\\testdata\\{pdfFile.FileName}", FileMode.Open, FileAccess.ReadWrite);
                imageConverter.Load(inputStream);

                // Convierte el PDF a imagen
                Stream outputStream = imageConverter.Convert(0, false, false);
                MemoryStream? stream = outputStream as MemoryStream;

                // Retorna el archivo como una imagen
                var newFn = Path.ChangeExtension(pdfFile.FileName, ".png");
                return Task.FromResult<IActionResult>(File(stream!.ToArray(), MediaTypeNames.Image.Png, $"{newFn}"));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(StatusCode(500, "Error al convertir el archivo PDF -- " + ex.Message));
            }
            finally
            {
                // Cierra el stream explícitamente
                if (inputStream != null)
                {
                    inputStream.Close();
                    inputStream.Dispose();
                }
            }
        }   
    }
}