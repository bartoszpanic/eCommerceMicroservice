namespace Product.Application.Dtos;

public class ProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}
