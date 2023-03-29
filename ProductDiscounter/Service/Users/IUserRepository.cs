using ProductDiscounter.Model.Users;

namespace ProductDiscounter.Service.Users;

public interface IUserRepository
{
    IEnumerable<User> GetAll();
    bool Add(User user);
    User Get(string name);
}
