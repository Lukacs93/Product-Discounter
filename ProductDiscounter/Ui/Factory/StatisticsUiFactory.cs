using ProductDiscounter.Service.Products.Statistics;

namespace ProductDiscounter.Ui.Factory;

public class StatisticsUiFactory : UiFactoryBase
{
    private readonly IProductStatistics _productStatistics;

    public StatisticsUiFactory(IProductStatistics productStatistics)
    {
        _productStatistics = productStatistics;
    }

    public override string UiName => "Statistics";

    public override UiBase Create()
    {
        return new StatisticsUi(_productStatistics, UiName, true);
    }
}
