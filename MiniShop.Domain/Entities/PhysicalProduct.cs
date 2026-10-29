
namespace MiniShop.Domain.Entities;

public class PhysicalProduct : Product
{
    public int StockQuantity { get; private set; }

    public decimal ShippingCost { get; private set; }

    public PhysicalProduct(int id,string name,decimal price,int stockQuantity,decimal shippingCost): base(id, name, price)
    {
        if (stockQuantity < 0)
            throw new ArgumentException("Количество на складе не может быть отрицательным.");

        if (shippingCost < 0)
            throw new ArgumentException("Стоимость доставки не может быть отрицательной.");

        StockQuantity = stockQuantity;
        ShippingCost = shippingCost;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Количество должно быть больше нуля.");

        StockQuantity += quantity;
    }


    public void ChangeStockQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Количество на складе не может быть отрицательным.");

        StockQuantity = newQuantity;
    }

    public void ChangeShippingCost(decimal newShippingCost)
    {
        if (newShippingCost < 0)
            throw new ArgumentException("Стоимость доставки не может быть отрицательной.");

        ShippingCost = newShippingCost;
    }


    public override decimal GetCalculatedFinalPrice()
    {
        return Price + ShippingCost;
    }

    public override string ToString()
    {
        return base.ToString() +$" | Доставка: {ShippingCost:F2}" +$" | Остаток: {StockQuantity}";
    }
}
