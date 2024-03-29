using Documents.Shared.Enums;

namespace Documents.Shared.Entities
{
    public class Archivos
    {
        public int ArchivoID { get; set; }
        public string NombreArchivo { get; set; } = null!;
        public FileType TipoArchivo { get; set; }
        public DateTime FechaSubida { get; set; }
    }
}