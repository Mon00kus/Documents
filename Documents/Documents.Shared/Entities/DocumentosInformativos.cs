
using System.Globalization;

namespace Documents.Shared.Entities
{
    public class DocumentosInformativos
    {
        public int ID { get; set; }
        public int ArchivoID { get; set; }
        public string? Descripcion { get; set; }
        public string? Resumen { set; get; }
        public string? Sentimiento { get; set; }
    }
}