using ProductDiscounter.Model.Offers;

namespace ProductDiscounter.Service.Offers;

public interface IOfferService
{
    public IEnumerable<Offer> GetOffers(DateTime date);
}
