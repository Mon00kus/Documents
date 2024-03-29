using Documents.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Documents.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Facturas> Factura { set; get; }
        public DbSet<Archivos> Archivo { set; get; }
        public DbSet<ProductosFactura> ProductosFactura { set; get; }
        public DbSet<DocumentosInformativos> DocumentosInformativos { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);         
        }
    }
}
