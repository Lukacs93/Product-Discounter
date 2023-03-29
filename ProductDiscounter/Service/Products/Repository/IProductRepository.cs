using ProductDiscounter.Model.Products;

namespace ProductDiscounter.Service.Products.Repository;

public interface IProductRepository
{
    IEnumerable<Product> AvailableProducts { get; }
    bool Add(IEnumerable<Product> products);
    bool SetProductAsSold(Product product);

}
