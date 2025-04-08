using Product.Infrastructure;
using Product.Application;
using Product.Application.Services;
using Product.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProductInfrastructure(builder.Configuration);
builder.Services.AddProductApplication();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "API for managing products in the eCommerce microservice."
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", async (IProductService service) =>
{
    return await service.GetProductsAsync();
})
.WithName("GetProducts")
.WithOpenApi();

app.MapPost("/product", async (IProductService service, [FromBody] ProductDto product) =>
{
    var result = await service.CreateProductAsync(product);
    return Results.Created($"product/{result}", result);
})
.WithName("CreateProduct")
.WithOpenApi();

app.MapDelete("/product/{id}", async (IProductService service, string id) =>
{
    var result = await service.SoftDeleteProductAsync(id);
    return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors.FirstOrDefault()?.Message);
})
.WithName("DeleteProduct")
.WithOpenApi();

app.Run();

