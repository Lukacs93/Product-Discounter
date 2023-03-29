using ProductDiscounter.Model.Products;

namespace ProductDiscounter.Model.Discounts;

public interface IDiscount
{
    bool Accepts(Product product, DateTime date);

    string Name { get; }

    int Rate { get; }
}
