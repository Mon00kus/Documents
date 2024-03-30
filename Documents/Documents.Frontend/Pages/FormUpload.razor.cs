using Microsoft.AspNetCore.Components.Forms;

namespace Documents.Frontend.Pages
{
    public partial class FormUpload
    {
        private List<IBrowserFile> uploadedFiles = new List<IBrowserFile>();
        private string previewSource = null!;
        private IBrowserFile currentFile = null!; // Guardará el archivo actual seleccionado para la vista previa

        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null)
            {
                // Configura el archivo actual seleccionado para vista previa
                currentFile = file;

                if (file.ContentType.StartsWith("image/"))
                {
                    // Tratamiento para imágenes
                    var imageFile = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                    previewSource = await ToImageSource(imageFile, file.ContentType);
                }
                else if (file.ContentType.Equals("application/pdf"))
                {
                    // Tratamiento para PDFs: Convertir a Base64 para mostrar en un <iframe>
                    previewSource = await ToBase64(file);
                    // Aquí podrías configurar cómo mostrar el PDF
                }
            }
        }

        private async Task AddToFileList()
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
        }

        private async Task<string> ToImageSource(IBrowserFile file, string format)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            return $"data:{format};base64,{Convert.ToBase64String(buffer)}";
        }

        private async Task<string> ToBase64(IBrowserFile file)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }
        private void ProceedFileList(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
