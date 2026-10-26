using InterviewAdvanced.Shopping;

namespace InterviewAdvanced.Tests;

public class ShoppingCartTests
{
    private static readonly CartProduct Keyboard = new("KB-1", "Keyboard", 50m);
    private static readonly CartProduct Mouse = new("MS-1", "Mouse", 20m);

    [Fact]
    public void AddItem_SameSkuTwice_MergesQuantity()
    {
        // Arrange
        var cart = new ShoppingCart(new NoDiscount());

        // Act
        cart.AddItem(Keyboard, 1);
        cart.AddItem(Keyboard, 2);

        // Assert
        var item = Assert.Single(cart.Items);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(150m, cart.Subtotal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddItem_NonPositiveQuantity_Throws(int quantity)
    {
        var cart = new ShoppingCart(new NoDiscount());

        Assert.Throws<ArgumentOutOfRangeException>(() => cart.AddItem(Keyboard, quantity));
    }

    [Fact]
    public void PercentageDiscount_IsAppliedToSubtotal()
    {
        var cart = new ShoppingCart(new PercentageDiscount(10));
        cart.AddItem(Keyboard, 2);
        cart.AddItem(Mouse, 1);

        Assert.Equal(120m, cart.Subtotal);
        Assert.Equal(12m, cart.Discount);
        Assert.Equal(108m, cart.Total);
    }

    [Theory]
    [InlineData(1, 0)]   // subtotal 50  -> below threshold
    [InlineData(2, 15)]  // subtotal 100 -> threshold reached
    public void ThresholdDiscount_OnlyAppliesAtOrAboveThreshold(int keyboards, decimal expectedDiscount)
    {
        var cart = new ShoppingCart(new ThresholdDiscount(threshold: 100m, amountOff: 15m));
        cart.AddItem(Keyboard, keyboards);

        Assert.Equal(expectedDiscount, cart.Discount);
    }

    [Fact]
    public void Discount_NeverExceedsSubtotal()
    {
        var cart = new ShoppingCart(new ThresholdDiscount(threshold: 0m, amountOff: 500m));
        cart.AddItem(Mouse, 1);

        Assert.Equal(0m, cart.Total);
    }

    [Theory]
    [InlineData("SAVE10", typeof(PercentageDiscount))]
    [InlineData("save10", typeof(PercentageDiscount))]
    [InlineData("BIG100", typeof(ThresholdDiscount))]
    [InlineData("nope", typeof(NoDiscount))]
    [InlineData(null, typeof(NoDiscount))]
    public void DiscountFactory_MapsCodesToStrategies(string? code, Type expected)
    {
        Assert.IsType(expected, DiscountFactory.FromCode(code));
    }
}
