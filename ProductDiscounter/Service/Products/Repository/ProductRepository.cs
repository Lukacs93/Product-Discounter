using ProductDiscounter.Model.Enums;
using ProductDiscounter.Model.Products;
using ProductDiscounter.Service.Logger;
using ProductDiscounter.Service.Persistence;

namespace ProductDiscounter.Service.Products.Repository;

public class ProductRepository : SqLiteConnector, IProductRepository
{
    private readonly string _tableName;

    public IEnumerable<Product> AvailableProducts => GetAvailableProducts();

    public ProductRepository(string dbFile, ILogger logger) : base(dbFile, logger)
    {
        _tableName = DatabaseManager.ProductsTableName;
    }

    private IEnumerable<Product> GetAvailableProducts()
    {
        var query =@$"SELECT * FROM {_tableName} WHERE sold = 'False'";
        var ret = new List<Product>();

        try
        {
            using var connection = GetPhysicalDbConnection();
            using var command = GetCommand(query, connection);
            using var reader = command.ExecuteReader();
            Logger.LogInfo($"{GetType().Name} executing query: {query}");

            while (reader.Read())
            {
                var product = new Product(reader.GetInt32(0), reader.GetInt32(1).ToString(), (Color)reader.GetInt32(2), (Season)reader.GetInt32(3), reader.GetInt32(4), reader.GetBoolean(5));
                ret.Add(product);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            throw;
        }

        return ret;
    }

    public bool Add(IEnumerable<Product> products)
    {
        // try
        // {
        //     var query = "";
        //     foreach (var product in products)
        //     {
        //         query = @$"INSERT INTO {_tableName} (name, color, season, price, sold) VALUES('{product.Name}', '{product.Color}', '{product.Season}', '{product.Price}', '{product.Sold}')";
        //         using var connection = GetPhysicalDbConnection();
        //         using var command = GetCommand(query, connection);
        //         using var reader = command.ExecuteReader();
        //         ExecuteNonQuery(query);
        //         Logger.LogInfo($"{GetType().Name} executing query: {query}");
        //     }
        //     return true;
        // }
        // catch (Exception e)
        // {
        //     Logger.LogError(e.Message);
        //     throw;
        // }
        
        try
        {
            var query = $"INSERT INTO {_tableName} (name, color, season, price, sold) VALUES";
            var values = new List<string>();
            foreach (var product in products)
            {
                values.Add($"('{product.Name}', '{product.Color}', '{product.Season}', '{product.Price}', '{product.Sold}')");
            }
            query += string.Join(",", values);
            using (var connection = GetPhysicalDbConnection())
            using (var command = GetCommand(query, connection))
            {
                ExecuteNonQuery(query);
                Logger.LogInfo($"{GetType().Name} executing query: {query}");
            }
            return true;
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            throw;
        }
    }

    public bool SetProductAsSold(Product product)
    {
        //Set the sold field in the database
        var query = @$"UPDATE {_tableName} SET sold = 'True' WHERE id = {product.Id}";
        try
        {
            using var connection = GetPhysicalDbConnection();
            using var command = GetCommand(query, connection);
            using var reader = command.ExecuteReader();
            Logger.LogInfo($"{GetType().Name} executing query: {query}");
            return ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            throw;
        }
        
    }
}
