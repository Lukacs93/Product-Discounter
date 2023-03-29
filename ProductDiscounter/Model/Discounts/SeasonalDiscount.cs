using ProductDiscounter.Extensions;
using ProductDiscounter.Model.Enums;
using ProductDiscounter.Model.Products;

namespace ProductDiscounter.Model.Discounts;

public record SeasonalDiscount(string Name, int Rate, int SeasonShift) : IDiscount
{
    public bool Accepts(Product product, DateTime date)
    {
        Season shiftedSeason = product.Season.Shift(SeasonShift);
        return shiftedSeason.Contains(date);
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Rate)}: {Rate}, {nameof(SeasonShift)}: {SeasonShift}";
    }
}
