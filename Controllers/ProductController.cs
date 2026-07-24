using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using cmkapi.DTO;
using cmkapi.Services.Interfaces;

namespace cmkapi.Controllers;

[ApiController]
[Route("products")]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllAsync());

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _productService.GetByIdAsync(id));

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request) => Ok(await _productService.CreateAsync(request));

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request) => Ok(await _productService.UpdateAsync(id, request));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);

        return Ok(new { success = true, message = "Product has been removed or inactive successfully" });   
    }
}