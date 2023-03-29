using ProductDiscounter.Model.Transactions;
using ProductDiscounter.Service.Authentication;
using ProductDiscounter.Service.Discounts;
using ProductDiscounter.Service.Discounts.Repository;
using ProductDiscounter.Service.Logger;
using ProductDiscounter.Service.Persistence;
using ProductDiscounter.Service.Products.Generator;
using ProductDiscounter.Service.Products.Repository;
using ProductDiscounter.Service.Transactions.Repository;
using ProductDiscounter.Service.Transactions.Simulator;
using ProductDiscounter.Service.Users;

namespace ProductDiscounter;

static class Program
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        ILogger logger = new ConsoleLogger();
        string dbFile = WorkDir + "\\Resources\\SeasonalProductDiscounter.db";

        IDatabaseManager dbManager = new DatabaseManager(dbFile, logger);

        dbManager.CreateTables();

        IProductRepository productRepository = new ProductRepository(dbFile, logger);
        IDiscountRepository discountRepository = new DiscountRepository();
        IUserRepository userRepository = new UserRepository(dbFile, logger);
        ITransactionRepository transactionRepository = new TransactionRepository(dbFile, logger);
        IAuthenticationService authenticationService = new AuthenticationService(userRepository);
        IDiscounterService discounterService = new DiscounterService(discountRepository);

        InitializeDatabase(productRepository);

        var simulator = new TransactionsSimulator(logger, userRepository, productRepository,
            authenticationService, discounterService, transactionRepository);


        RunSimulation(simulator, productRepository, transactionRepository);

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    private static void InitializeDatabase(IProductRepository productRepository)
    {
        if (!productRepository.AvailableProducts.Any())
        {
            var randomProductGenerator = new RandomProductGenerator(1000, 20, 80);
            productRepository.Add(randomProductGenerator.Products);
            //add products to repo
        }
    }

    private static void RunSimulation(TransactionsSimulator simulator, IProductRepository productRepository, ITransactionRepository transactionRepository)
    {
        int days = 0;
        var date = DateTime.Today;

        // set your own condition
        while (productRepository.AvailableProducts.Count() > 0)
        {
            Console.WriteLine("Starting simulation...");
            simulator.Run(new TransactionsSimulatorSettings(date, 1000, 500));

            var transactions = transactionRepository.GetAll().ToList();

            Console.WriteLine($"{date} ended, total transactions: {transactions.Count}, total income: {transactions.Sum(t => t.PricePaid)}");
            Console.WriteLine($"Products left to sell: {productRepository.AvailableProducts.Count()}");
        }
    }
}