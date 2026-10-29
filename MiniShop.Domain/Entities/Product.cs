
namespace MiniShop.Domain.Entities;

public abstract class Product
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    protected Product(int id, string name, decimal price)
    {
        if (id < 0)
            throw new ArgumentException("ID должен быть положительным.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название не может быть пустым.");

        if (price <= 0)
            throw new ArgumentException("Цена должна быть больше нуля.");

        Id = id;
        Name = name;
        Price = price;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Цена должна быть больше нуля.");

        Price = newPrice;
    }

    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Название товара не может быть пустым.");

        Name = newName;
    }


    public abstract decimal GetCalculatedFinalPrice();

    public override string ToString()
    {
        return $"ID: {Id} | {Name} | " + $"Цена: {GetCalculatedFinalPrice():F2}";
    }
}
