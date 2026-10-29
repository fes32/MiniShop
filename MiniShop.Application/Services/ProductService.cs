using MiniShop.Application.DTOs;
using MiniShop.Application.Interfaces;
using MiniShop.Domain.Entities;

namespace MiniShop.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Product> products =
            await _repository.GetAllAsync(cancellationToken);

        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        Product? product = await _repository.GetByIdAsync(
            id, cancellationToken);

        return product is null ? null : MapToDto(product);
    }


    public async Task<ProductDto> CreateAsync(string name,decimal price,string productType,int stockQuantity,decimal shippingCost,int licenseDurationDays,CancellationToken cancellationToken = default)
    {
        Product product;

        if (productType == "Physical")
            product = new PhysicalProduct(id: 0,name: name,price: price,stockQuantity: stockQuantity,shippingCost: shippingCost);
        else if (productType == "Digital")
            product = new DigitalProduct(id: 0,name: name,price: price,licenseDurationDays: licenseDurationDays);
        else
            throw new ArgumentException("Неизвестный тип товара.",nameof(productType));

        await _repository.AddAsync(product, cancellationToken);

        return MapToDto(product);
    }


    public async Task<ProductDto?> UpdateAsync(int id,string name,decimal price,int stockQuantity,decimal shippingCost,int licenseDurationDays,CancellationToken cancellationToken = default)
    {
        Product? product = await _repository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return null;

        product.ChangeName(name);
        product.ChangePrice(price);

        if (product is PhysicalProduct physicalProduct)
        {
            physicalProduct.ChangeStockQuantity(stockQuantity);
            physicalProduct.ChangeShippingCost(shippingCost);
        }
        else if (product is DigitalProduct digitalProduct)
        {
            digitalProduct.ChangeLicenseDurationDays(licenseDurationDays);
        }

        await _repository.UpdateAsync(product, cancellationToken);

        return MapToDto(product);
    }


    public async Task<bool> DeleteAsync(int id,CancellationToken cancellationToken = default)
    {
        Product? product = await _repository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return false;

        await _repository.DeleteAsync(product, cancellationToken);

        return true;
    }

    private static ProductDto MapToDto(Product product)
    {
        ProductDto dto = new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            FinalPrice = product.GetCalculatedFinalPrice()
        };

        if (product is PhysicalProduct physicalProduct)
        {
            dto.ProductType = "Physical";
            dto.StockQuantity = physicalProduct.StockQuantity;
            dto.ShippingCost = physicalProduct.ShippingCost;
        }
        else if (product is DigitalProduct digitalProduct)
        {
            dto.ProductType = "Digital";
            dto.LicenseDurationDays = digitalProduct.LicenseDurationDays;
        }

        return dto;
    }


    public async Task<bool> PurchaseAsync(List<PurchaseItemDto> items,CancellationToken cancellationToken = default)
    {
        if (items is null || items.Count == 0)
            return false;

        if (items.Any(item =>item.ProductId <= 0 || item.Quantity <= 0))
            return false;

        // Не допускаем повторения одного товара в запросе.
        if (items.Select(item => item.ProductId).Distinct().Count()!= items.Count)
            return false;

        var products = await _repository.GetAllAsync(cancellationToken);

        var selectedProducts = products.Where(product => items.Any(item => item.ProductId == product.Id)).ToList();

        // Проверяем, что все товары существуют.
        if (selectedProducts.Count != items.Count)
            return false;

        // Сначала проверяем все товары, не изменяя базу.
        foreach (var item in items)
        {
            var product = selectedProducts.First(p => p.Id == item.ProductId);

            if (product is PhysicalProduct physicalProduct)
            {
                if (physicalProduct.StockQuantity < item.Quantity)
                    return false;
            }
        }

        // Выполняем покупку.
        foreach (var item in items)
        {
            var product = selectedProducts.First(p => p.Id == item.ProductId);

            if (product is PhysicalProduct physicalProduct)
            {
                var remainingQuantity = physicalProduct.StockQuantity - item.Quantity;

                if (remainingQuantity == 0)
                {
                    await _repository.DeleteAsync(product,cancellationToken);
                }
                else
                {
                    physicalProduct.ChangeStockQuantity(remainingQuantity);
                    await _repository.UpdateAsync(product,cancellationToken);
                }
            }
            else
            {
                // Цифровой товар удаляется после покупки.
                await _repository.DeleteAsync(product, cancellationToken);
            }
        }

        return true;
    }

}
