using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI;
using Shared.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductsAPIContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductsController(ProductsAPIContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
    {
        return await _context.Product.ToListAsync();
    }

    // GET: api/Product/5
    [HttpGet("{productid}")]
    public async Task<ActionResult<Product>> GetProduct(int productid)
    {
        var product = await _context.Product.FindAsync(productid);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // PUT: api/Product/5
    [HttpPut("{productid}")]
    public async Task<IActionResult> PutProduct(int? productid, Product product)
    {
        if (productid != product.ProductId)
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
            if (!ProductExists(productid))
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

    // POST: api/Product
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Product.Add(product);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish<ProductCreated>(new ProductCreated
        {
            Id = product.ProductId,
            Name = product.Name
        });

        return CreatedAtAction("GetProduct", new { productid = product.ProductId }, product);
    }

    // DELETE: api/Product/5
    [HttpDelete("{productid}")]
    public async Task<IActionResult> DeleteProduct(int? productid)
    {
        var product = await _context.Product.FindAsync(productid);
        if (product == null)
        {
            return NotFound();
        }

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int? productid)
    {
        return _context.Product.Any(e => e.ProductId == productid);
    }
}