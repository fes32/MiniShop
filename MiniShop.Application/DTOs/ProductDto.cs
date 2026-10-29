namespace MiniShop.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ProductType { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public decimal ShippingCost { get; set; }

    public int LicenseDurationDays { get; set; }

    public decimal FinalPrice { get; set; }
}
