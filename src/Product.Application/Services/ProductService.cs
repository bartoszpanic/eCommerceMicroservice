using FluentResults;
using Product.Application.Dtos;

namespace Product.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<string>> CreateProductAsync(ProductDto product)
    {
        return await _productRepository.CreateProductAsync(product);
    }

    public async Task<List<Shared.Product>> GetProductsAsync()
    {
        return await _productRepository.GetProductsAsync();
    }

    public async Task<Result> SoftDeleteProductAsync(string id)
    {
        return await _productRepository.SoftDeleteProductAsync(id);
    }
}
