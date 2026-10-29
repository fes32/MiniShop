
namespace MiniShop.Application.DTOs;

public class PurchaseRequest
{
    public List<PurchaseItemDto> Items { get; set; } = new();
}

public class PurchaseItemDto
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
