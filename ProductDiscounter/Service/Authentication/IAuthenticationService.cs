using ProductDiscounter.Model.Users;

namespace ProductDiscounter.Service.Authentication;

public interface IAuthenticationService
{
    bool Authenticate(User user);
}
