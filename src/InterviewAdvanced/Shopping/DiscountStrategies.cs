namespace InterviewAdvanced.Shopping;

// Strategy pattern + Open/Closed: add a new discount by adding a class,
// without touching ShoppingCart.
public interface IDiscountStrategy
{
    decimal Calculate(ShoppingCart cart);
}

public sealed class NoDiscount : IDiscountStrategy
{
    public decimal Calculate(ShoppingCart cart) => 0m;
}

public sealed class PercentageDiscount : IDiscountStrategy
{
    private readonly decimal _percent;

    public PercentageDiscount(decimal percent)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(percent);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(percent, 100);
        _percent = percent;
    }

    public decimal Calculate(ShoppingCart cart) => Math.Round(cart.Subtotal * _percent / 100m, 2);
}

// Fixed amount off once the subtotal reaches a threshold, e.g. $10 off orders over $100.
public sealed class ThresholdDiscount(decimal threshold, decimal amountOff) : IDiscountStrategy
{
    public decimal Calculate(ShoppingCart cart) => cart.Subtotal >= threshold ? amountOff : 0m;
}

// Single Responsibility: turning a promo code into a strategy lives here, not in the cart.
public static class DiscountFactory
{
    public static IDiscountStrategy FromCode(string? code) => code?.ToUpperInvariant() switch
    {
        "SAVE10" => new PercentageDiscount(10),
        "BIG100" => new ThresholdDiscount(threshold: 100m, amountOff: 15m),
        _ => new NoDiscount()
    };
}
