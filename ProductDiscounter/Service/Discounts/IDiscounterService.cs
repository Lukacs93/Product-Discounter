using ProductDiscounter.Model.Offers;
using ProductDiscounter.Model.Products;

namespace ProductDiscounter.Service.Discounts;

public interface IDiscounterService
{
    Offer GetOffer(Product product, DateTime date);
}
