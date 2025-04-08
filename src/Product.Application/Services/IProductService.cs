using FluentResults;
using Product.Application.Dtos;


namespace Product.Application.Services;

public interface IProductService
{
    Task<List<Shared.Product>> GetProductsAsync();
    Task<Result<string>> CreateProductAsync(ProductDto product);
    Task<Result> SoftDeleteProductAsync(string id);
    Task<Result<Shared.Product>> GetProductByIdAsync(string id);
}

