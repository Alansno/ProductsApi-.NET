using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Context;
using ProductsApi.Models;
using ProductsApi.Models.Dto;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("profile")]
        public async Task<ActionResult> GetUserProfile()
        {
            // Obtener el principal del contexto actual
            ClaimsPrincipal principal = HttpContext.User;

            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            // Obtener los claims del principal
            var claims = principal.Claims;

            // Obtener un claim específico, como el nombre del usuario
            var username = principal.FindFirst(ClaimTypes.Name)?.Value;

            // También puedes acceder a otros claims, como el ID del usuario
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Aquí puedes hacer lo que quieras con los datos del usuario
            // Por ejemplo, devolverlos en la respuesta
            return Ok(new { Username = username, UserId = userId });
        }

        // GET: api/Products/5
        [HttpGet()]
        [Route("user")]
        public async Task<ActionResult> GetProduct()
        {
            ClaimsPrincipal principal = HttpContext.User;
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId == null) return BadRequest();

            var productsUser = await _context.Products.Where(u => u.UserId == userId)
                .Select(u => new {u.Name,u.Price}).ToListAsync();

            if (productsUser == null) return NotFound();

            return Ok(productsUser);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("crear")]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            ClaimsPrincipal principal = HttpContext.User;
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId == null) return BadRequest();

            var product = new Product()
            {
                Name = productDto.Name,
                Price = productDto.Price,
                UserId = userId
            };

            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Producto creado correctamente" });
            }
            catch (DbUpdateConcurrencyException ex) { 
                return BadRequest(ex);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
