using ProductDiscounter.Model.Discounts;

namespace ProductDiscounter.Service.Discounts.Repository;

public interface IDiscountRepository
{
    IEnumerable<IDiscount> Discounts { get; }
}
