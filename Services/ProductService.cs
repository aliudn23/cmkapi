using Microsoft.EntityFrameworkCore;
using cmkapi.DTO;
using cmkapi.Services.Interfaces;
using cmkapi.Data;
using cmkapi.Model;

namespace cmkapi.Services;

public class ProductService(ApplicationDbContext context) : IProductService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        if (request.Name == string.Empty)
            throw new Exception("Name of product is required");
        
        if (request.Price <= 0)
            throw new Exception("Price must be greater than zero");

        if (request.Stock < 1)
            throw new Exception("Stock at least have one");

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
        };
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new Exception("Product not found");

        if (!product.IsActive)
            throw new Exception("Product has been removed or inactive");

        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        return await _context.Products
        .Where(x => x.IsActive == true)
        .Select(x => new ProductResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock
        })
        .OrderBy(x => x.Id)
        .ToListAsync();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        return await _context.Products
        .Where(x => x.Id == id && x.IsActive == true)
        .Select(x => new ProductResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            Stock = x.Stock
        })
        .FirstOrDefaultAsync();
    }

    public async Task<ProductResponse> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new Exception("Product not found");

        if (request.Name == string.Empty)
            throw new Exception("Name of product is required");
        
        if (request.Price <= 0)
            throw new Exception("Price must be greater than zero");

        if (request.Stock < 1)
            throw new Exception("Stock at least have one");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}