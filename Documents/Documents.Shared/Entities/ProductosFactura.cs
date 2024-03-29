
namespace Documents.Shared.Entities
{
    public  class ProductosFactura
    {
        public int ID { get; set; }
        public int FacturaID { get; set;}
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
