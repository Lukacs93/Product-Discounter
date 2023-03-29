namespace ProductDiscounter.Model.Products;

public record PriceRange(double Minimum, double Maximum)
{
    public bool Contains(double price) => price > Minimum && price < Maximum;
}
