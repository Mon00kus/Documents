
namespace Documents.Shared.Entities
{
    public  class Facturas
    {
        public int FacturaID { get; set; }
        public int ArchivoID { get; set; }
        public string? Cliente { get; set; }
        public string? Proveedor { get; set; }
        public int NumeroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Totalfactura { get; set; }
    }
}