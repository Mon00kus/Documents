
using Microsoft.AspNetCore.Mvc;
using Tesseract;

namespace Documents.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtractTextFromImageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ExtractTextFromImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getTesseractData")]
        public ActionResult<string> Get(string imagePath)
        {
            var tessData = _configuration["W0rkingPath:TessData"];            
            try
            {
                using (var engine = new TesseractEngine($"{tessData}", "spa", EngineMode.Default)) //@"./tessdata"
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}