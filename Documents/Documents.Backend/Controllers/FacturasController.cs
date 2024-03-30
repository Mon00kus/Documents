using Documents.Backend.Data;
using Documents.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Documents.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase    
    {
        private readonly DataContext _context;

        public FacturasController(DataContext context)
        {
            _context = context;
        }        
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Facturas.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {            
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return Ok(factura);
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(Factura factura)
        {
            _context.Add(factura);
            await _context.SaveChangesAsync();
            return Ok(factura);
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(Factura factura)
        {
            _context.Update(factura);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}