
namespace Documents.Shared.Entities
{
    public  class Factura
    {
        public int ID { get; set; } 
        public int ArchivoID { get; set; }
        public string? Cliente { get; set; }
        public string? Proveedor { get; set; }
        public string NumeroFactura { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public decimal Totalfactura { get; set; }
    }
}