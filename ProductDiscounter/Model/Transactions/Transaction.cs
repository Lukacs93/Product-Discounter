using ProductDiscounter.Model.Products;
using ProductDiscounter.Model.Users;

namespace ProductDiscounter.Model.Transactions;

public record Transaction(int Id, DateTime Date, User User, Product Product, double PricePaid);
