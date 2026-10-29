
using MiniShop.Application.DTOs;

namespace MiniShop.Application.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ProductDto?> GetByIdAsync(int id,CancellationToken cancellationToken = default);

    Task<ProductDto> CreateAsync(string name, decimal price, string productType, int stockQuantity, decimal shippingCost, int licenseDurationDays, CancellationToken cancellationToken = default);


    Task<ProductDto?> UpdateAsync(int id,string name,decimal price,int stockQuantity,decimal shippingCost,int licenseDurationDays,CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id,CancellationToken cancellationToken = default);


    Task<bool> PurchaseAsync(List<PurchaseItemDto> items,CancellationToken cancellationToken = default);

}


