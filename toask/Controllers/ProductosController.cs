using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using toask.Data;
using toask.Models;

namespace toask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProductosController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            if (_dataContext == null)
            {
                return NotFound();
            }
            return await _dataContext.Productos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _dataContext.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CreateProducto(Producto producto)
        {
            _dataContext.Productos.Add(producto);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            _dataContext.Entry(producto).State = EntityState.Modified;

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _dataContext.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            _dataContext.Productos.Remove(producto);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Producto>>> FilterByNombreOrPrecio(string nombre = null, decimal? precio = null)
        {
            var productosFiltrados = await _dataContext.Productos
                .Where(p => nombre == null || p.Nombre.Contains(nombre))
                .Where(p => precio == null || p.Precio == precio)
                .ToListAsync();

            if (productosFiltrados.Count == 0)
            {
                return NotFound();
            }

            return productosFiltrados;
        }

        private bool ProductoExists(int id)
        {
            return _dataContext.Productos.Any(e => e.Id == id);
        }
    }
}
