using ProductDiscounter.Model.Transactions;

namespace ProductDiscounter.Service.Transactions.Repository;

public interface ITransactionRepository
{
    bool Add(Transaction transaction);
    IEnumerable<Transaction> GetAll();
}
