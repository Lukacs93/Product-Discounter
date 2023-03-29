using System.Data;
using ProductDiscounter.Model.Products;
using ProductDiscounter.Model.Transactions;
using ProductDiscounter.Model.Users;
using ProductDiscounter.Service.Logger;
using ProductDiscounter.Service.Persistence;
using ProductDiscounter.Utilities;

namespace ProductDiscounter.Service.Transactions.Repository;

public class TransactionRepository : SqLiteConnector, ITransactionRepository
{
    private readonly string _tableName;

    public TransactionRepository(string dbFile, ILogger logger) : base(dbFile, logger)
    {
        _tableName = DatabaseManager.TransactionsTableName;
    }

    public bool Add(Transaction transaction)
    {
        var query = $@"INSERT INTO {DatabaseManager.TransactionsTableName} (dates, user_id, product_id, price_paid) VALUES (@Date, @userId, @productId, @pricePaid)";
        
        try
        {
            using var connection = GetPhysicalDbConnection();
            using var command = GetCommand(query, connection);
            command.Parameters.AddWithValue("@Date", transaction.Date);
            command.Parameters.AddWithValue("@userId", transaction.User.Id);
            command.Parameters.AddWithValue("@productId", transaction.Product.Id);
            command.Parameters.AddWithValue("@pricePaid", transaction.PricePaid);
            Logger.LogInfo($"{GetType().Name} executing query: {query}");
            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected == 1;
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            throw;
        }
    }

    public IEnumerable<Transaction> GetAll()
    {
        var query = @$"SELECT t.*, u.*, p.* 
                  FROM {_tableName} t 
                  INNER JOIN {DatabaseManager.UsersTableName} u ON t.user_id = u.id 
                  INNER JOIN {DatabaseManager.ProductsTableName} p ON t.product_id = p.id";

        try
        {
            using var connection = GetPhysicalDbConnection();
            using var command = GetCommand(query, connection);
            using var reader = command.ExecuteReader();
            Logger.LogInfo($"{GetType().Name} executing query: {query}");


            var dt = new DataTable();

            //This is required otherwise the DataTable tries to force the DB constrains on the result set, which can cause problems in some cases (e.g. UNIQUE)
            using var ds = new DataSet { EnforceConstraints = false };
            ds.Tables.Add(dt);
            dt.Load(reader);
            ds.Tables.Remove(dt);

            var lst = new List<Transaction>();
            foreach (DataRow row in dt.Rows)
            {
                var user = ToUser(row);
                var product = ToProduct(row);
                var transaction = ToTransaction(row, user, product);

                lst.Add(transaction);
            }

            return lst;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static User ToUser(DataRow row)
    {
        var id = TypeConverters.ToInt(row["id"]);
        var name = TypeConverters.ToString(row["name"]);
        var password = TypeConverters.ToString(row["password"]);

        return new User(id, name, password);
    }

    private static Product ToProduct(DataRow row)
    {
        var id = TypeConverters.ToInt(row["id"]);
        var name = TypeConverters.ToString(row["name"]);
        var color = TypeConverters.GetColorEnum(row["color"].ToString());
        var season = TypeConverters.GetSeasonEnum(row["season"].ToString());
        var price = TypeConverters.ToDouble(row["price"]);
        var sold = Convert.ToString(row["sold"]);
        if (sold == "False")
        {
            return new Product(id, name, color, season, price, false); 
        }
        
        return new Product(id, name, color, season, price, true);

    }

    private static Transaction ToTransaction(DataRow row, User user, Product product)
    {
        var id = TypeConverters.ToInt(row["id"]);
        var date = TypeConverters.ToDateTime(row["dates"].ToString());
        var pricePaid = TypeConverters.ToDouble(row["price_paid"]);
        
        return new Transaction(id, date, user, product, pricePaid);
    }
}
