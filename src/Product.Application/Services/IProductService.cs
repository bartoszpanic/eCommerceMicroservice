using Shared;

namespace Product.Application.Services;

public interface IProductService
{
    Task<List<Shared.Product>> GetProductsAsync();
}
