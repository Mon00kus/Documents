using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http;

namespace Documents.Frontend.Pages.FormUploads
{
    public partial class FormUploads
    {
        private List<IBrowserFile> uploadedFiles = new List<IBrowserFile>();
        private List<ForUploadFile> uploadedConvertedFiles = new List<ForUploadFile>();
        private string previewSource = null!;
        private IBrowserFile currentFile = null!; // Guardará el archivo actual seleccionado para la vista previa        


        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            IBrowserFile imageFile = null!;
            var file = e.File;
            if (file != null)
            {
                currentFile = file;
                if (file.ContentType.StartsWith("image/"))
                {
                    // Tratamiento para imágenes
                    imageFile = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                    previewSource = await ToImageSource(imageFile);
                }

                if (file.ContentType.Equals("application/pdf"))
                {
                    // Llama al backend para convertir el PDF a PNG
                    var responseStream = await ConvertToStream(file);
                    // Convierte el stream a base64
                    var base64Image = await StreamToBase64(responseStream);
                    // Establece la fuente de la imagen para la vista previa
                    previewSource = $"data:image/png;base64,{base64Image}";
                }
            }
        }

        private async Task<string> StreamToBase64(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            return Convert.ToBase64String(bytes);
        }

        private async Task<Stream> ConvertToStream(IBrowserFile file)
        {

            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 15)); // Tamaño máximo del archivo de 15MB
            content.Add(fileContent, "pdfFile", file.Name);

            // Enviar el archivo al backend
            var response = await httpClient.PostAsync("api/ConvertPdfToImage", content);

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como un Stream
                return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                // Manejar la situación en la que la respuesta no es exitosa
                throw new InvalidOperationException("No se pudo obtener el stream del archivo convertido.");
            }
        }

        private Task AddConvertedFileToFileList()
        {
            // Cambia la extensión del nombre del archivo a .png
            var fileNameWithPngExtension = Path.ChangeExtension(currentFile.Name, ".png");

            // Crea un nuevo objeto UploadedFile con el nuevo nombre
            var uploadedConvertedFile = new ForUploadFile
            {
                Name = fileNameWithPngExtension,
                Size = currentFile.Size,
                ContentType = "image/png", // Asumiendo que ahora es una imagen PNG
                TheStream = currentFile.OpenReadStream(),
            };

            // Añade el nuevo objeto a la lista si no existe uno con el mismo nombre
            if (!uploadedConvertedFiles.Any(f => f.Name!.Equals(uploadedConvertedFile.Name, StringComparison.OrdinalIgnoreCase)))
            {
                uploadedConvertedFiles.Add(uploadedConvertedFile);

            }

            // Limpia la vista previa y el archivo actual seleccionado
            previewSource = null!;
            currentFile = null!;
            StateHasChanged(); // Actualiza el estado para reflejar los cambios en la UI

            return Task.CompletedTask;
        }

        private Task AddToFileList()
        {
            // Verifica si el archivo ya está en la lista por nombre

            if (!uploadedFiles.Any(f => f.Name.Equals(currentFile.Name, StringComparison.OrdinalIgnoreCase)))
            {
                uploadedFiles.Add(currentFile); // Añade el archivo a la lista si no está duplicado
            }
            // Limpia la vista previa y el archivo actual seleccionado
            previewSource = null!;
            currentFile = null!;
            StateHasChanged(); // Actualiza el estado para reflejar los cambios en la UI
            return Task.CompletedTask;            
        }


        private async Task<string> ToImageSource(IBrowserFile file)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }

        private async Task<string> ToBase64(IBrowserFile file)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);

            return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }

        private void ProceedFileList(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public class ForUploadFile
        {
            public string? Name { get; set; }
            public long Size { get; set; }
            public string? ContentType { get; set; }
            public DateTime LastModified { get; set; }
            public Stream? TheStream { get; set; }

        }
    }
}
