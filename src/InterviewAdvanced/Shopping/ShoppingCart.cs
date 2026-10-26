namespace InterviewAdvanced.Shopping;

public record CartProduct(string Sku, string Name, decimal UnitPrice);

public class CartItem
{
    public CartProduct Product { get; }
    public int Quantity { get; private set; }
    public decimal LineTotal => Product.UnitPrice * Quantity;

    public CartItem(CartProduct product, int quantity)
    {
        Product = product;
        Increase(quantity);
    }

    public void Increase(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        Quantity += quantity;
    }
}

// Encapsulation: the item list can only change through AddItem/RemoveItem,
// so the cart can enforce its own rules (no negative quantities, etc.).
// Dependency Inversion: the cart depends on IDiscountStrategy, not a concrete discount.
public class ShoppingCart(IDiscountStrategy discount)
{
    private readonly Dictionary<string, CartItem> _items = new();

    public IReadOnlyCollection<CartItem> Items => _items.Values;

    public decimal Subtotal => _items.Values.Sum(i => i.LineTotal);

    public decimal Discount => Math.Min(discount.Calculate(this), Subtotal);

    public decimal Total => Subtotal - Discount;

    public void AddItem(CartProduct product, int quantity = 1)
    {
        if (_items.TryGetValue(product.Sku, out var existing))
            existing.Increase(quantity);
        else
            _items[product.Sku] = new CartItem(product, quantity);
    }

    public bool RemoveItem(string sku) => _items.Remove(sku);
}
