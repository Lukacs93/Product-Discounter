using ProductDiscounter.Service.Products.Browser;

namespace ProductDiscounter.Ui.Factory;

public class ProductsUiFactory : UiFactoryBase
{
    private readonly ProductBrowser _productBrowser;

    public ProductsUiFactory(ProductBrowser productBrowser)
    {
        _productBrowser = productBrowser;
    }

    public override string UiName => "Products";

    public override UiBase Create()
    {
        return new ProductsUi(_productBrowser, UiName, true);
    }
}
