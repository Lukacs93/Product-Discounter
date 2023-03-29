using ProductDiscounter.Model.Users;
using ProductDiscounter.Service.Logger;
using ProductDiscounter.Service.Persistence;

namespace ProductDiscounter.Service.Users;

public class UserRepository : SqLiteConnector, IUserRepository
{
    public UserRepository(string dbFile, ILogger logger) : base(dbFile, logger)
    {
    }

    public IEnumerable<User> GetAll()
    {
        var query = @$"SELECT * FROM {DatabaseManager.UsersTableName}";
        var ret = new List<User>();

        try
        {
            using var connection = GetPhysicalDbConnection();
            using var command = GetCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                ret.Add(user);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            throw;
        }

        return ret;
    }

    public bool Add(User user)
    {
        // var query = @$"INSERT INTO {DatabaseManager.UsersTableName} (user_name, password) VALUES('{user.UserName}', '{user.Password}')";
        // try
        // {
        //     using var connection = GetPhysicalDbConnection();
        //     using var command = GetCommand(query, connection);
        //     using var reader = command.ExecuteReader();
        //     ExecuteNonQuery(query);
        //     return true;
        // }
        // catch (Exception e)
        // {
        //     Logger.LogError(e.Message);
        //     throw;
        // }
        
        var query = $"INSERT INTO {DatabaseManager.UsersTableName} (user_name, password) VALUES(@user_name, @password)";
        try
        {
            using (var connection = GetPhysicalDbConnection())
            using (var command = GetCommand(query, connection))
            {
                command.Parameters.AddWithValue("@user_name", user.UserName);
                command.Parameters.AddWithValue("@password", user.Password);
                command.ExecuteNonQuery();
                Logger.LogInfo($"{GetType().Name} executing query: {query}");
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("NOT REGISTERED THE USER");
            Logger.LogError(e.Message);
            throw;
        }
    }

    public User Get(string name)
    {
        // var query = @$"SELECT * FROM {DatabaseManager.UsersTableName} WHERE user_name = {name}";
        // User user = null;
        // try
        // {
        //     using var connection = GetPhysicalDbConnection();
        //     using var command = GetCommand(query, connection);
        //     using var reader = command.ExecuteReader();
        //
        //     while (reader.Read())
        //     {
        //         user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
        //     }
        //     Logger.LogInfo($"{GetType().Name} executing query: {query}");
        //     return user;
        // }
        // catch (Exception e)
        // {
        //     Logger.LogError(e.Message);
        //     throw;
        // }
        var query = $"SELECT * FROM {DatabaseManager.UsersTableName} WHERE user_name = @name";
        User user = null;
        try
        {
            using var connection = GetPhysicalDbConnection();
            using var command =  GetCommand(query, connection);
            command.Parameters.AddWithValue("@name", name);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
            }
            else
            {
                Logger.LogInfo($"No user found with the name {name}");
            }
            Logger.LogInfo($"{GetType().Name} executing query: {query}");
            return user;
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            return user;
        }
    }
}
