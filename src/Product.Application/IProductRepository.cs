namespace Product.Application;

public interface IProductRepository
{
    Task<List<Shared.Product>> GetProductsAsync();
}
