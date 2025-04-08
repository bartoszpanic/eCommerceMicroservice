using FluentResults;
using Product.Application.Dtos;

namespace Product.Application;

public interface IProductRepository
{
    Task<List<Shared.Product>> GetProductsAsync();
    Task<Result<string>> CreateProductAsync(ProductDto product);
    Task<Result> SoftDeleteProductAsync(string id);
}
