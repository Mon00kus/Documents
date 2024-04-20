using Documents.Shared.Enums;

namespace Documents.Shared.Entities
{
    public class Archivo
    {
        public int ID { get; set; }
        public string NombreArchivo { get; set; } = null!;
        public FileType TipoArchivo { get; set; }
        public string IA { set; get; } = null!;
        public DateTime FechaSubida { get; set; }
    }
}