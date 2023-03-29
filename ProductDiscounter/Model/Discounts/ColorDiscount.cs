using ProductDiscounter.Extensions;
using ProductDiscounter.Model.Enums;
using ProductDiscounter.Model.Products;

namespace ProductDiscounter.Model.Discounts;

public record ColorDiscount(string Name, int Rate, Color Color, Season Season) : IDiscount
{
    public bool Accepts(Product product, DateTime date)
    {
        return Color == product.Color && Season.Contains(date);
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Rate)}: {Rate}, {nameof(Color)}: {Color}, {nameof(Season)}: {Season}";
    }
}
