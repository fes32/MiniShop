using MiniShop.Application.DTOs;

namespace MiniShop.Web.Models;

public class CartItem
{
    public ProductDto Product { get; set; } = null!;

    public int Quantity { get; set; } = 1;

    public decimal TotalPrice => Product.FinalPrice * Quantity;
}
