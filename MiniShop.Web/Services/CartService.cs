
using MiniShop.Application.DTOs;
using MiniShop.Web.Models;

namespace MiniShop.Web.Services;

public class CartService
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public int TotalItems => _items.Sum(item => item.Quantity);

    public decimal TotalPrice => _items.Sum(item => item.TotalPrice);

    public event Action? OnChange;

    public void Add(ProductDto product)
    {
        if (product.ProductType == "Physical" && product.StockQuantity <= 0)
        {
            throw new InvalidOperationException("Товара нет в наличии.");
        }

        var existingItem = _items.FirstOrDefault(item => item.Product.Id == product.Id);

        if (existingItem is not null)
        {
            if (product.ProductType == "Physical" &&existingItem.Quantity >= product.StockQuantity)
            {
                throw new InvalidOperationException("Нельзя добавить больше товара, чем есть на складе.");
            }

            existingItem.Quantity++;
        }
        else
        {
            _items.Add(new CartItem
            {
                Product = product,
                Quantity = 1
            });
        }

        NotifyChanged();
    }

    public void Remove(int productId)
    {
        _items.RemoveAll(item => item.Product.Id == productId);
        NotifyChanged();
    }

    public void ChangeQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(item => item.Product.Id == productId);

        if (item is null)
            return;

        if (quantity <= 0)
        {
            Remove(productId);
            return;
        }

        if (item.Product.ProductType == "Physical" && quantity > item.Product.StockQuantity)
        {
            throw new InvalidOperationException("Количество превышает остаток на складе.");
        }

        item.Quantity = quantity;
        NotifyChanged();
    }

    public void Clear()
    {
        _items.Clear();
        NotifyChanged();
    }

    private void NotifyChanged()
    {
        OnChange?.Invoke();
    }
}
