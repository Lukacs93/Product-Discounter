using ProductDiscounter.Model.Discounts;
using ProductDiscounter.Model.Products;

namespace ProductDiscounter.Model.Offers;

public record Offer(Product Product, DateTime Date, IEnumerable<IDiscount> Discounts, double Price)
{
    public override string ToString()
    {
        return
            $"{nameof(Product)}: {Product}, " +
            $"{nameof(Date)}: {Date}, " +
            $"{nameof(Discounts)}: {string.Join(",", Discounts)}, " +
            $"{nameof(Price)}: {Price}";
    }
}
