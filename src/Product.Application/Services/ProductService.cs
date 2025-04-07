using System;

namespace Product.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Shared.Product>> GetProductsAsync()
    {
        return await _productRepository.GetProductsAsync();
    }
}
