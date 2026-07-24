using cmkapi.DTO;

namespace cmkapi.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync();

    Task<ProductResponse?> GetByIdAsync(int id);

    Task<ProductResponse> CreateAsync(CreateProductRequest request);

    Task<ProductResponse> UpdateAsync(int id, UpdateProductRequest request);

    Task DeleteAsync(int id);
}