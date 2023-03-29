using ProductDiscounter.Model.Users;
using ProductDiscounter.Service.Users;

namespace ProductDiscounter.Service.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public bool Authenticate(User user)
    {
        try
        {
            var authenticatedUser = _userRepository.Get(user.UserName);
            if (authenticatedUser.Password == user.Password && authenticatedUser.UserName == user.UserName)
            {
                Console.WriteLine("SUCSESSFULL AUTHENTICATON");
                return true;
            }

            Console.WriteLine("Unsuccessful Authentication");
            return false;
        }
        catch (Exception e)
        {
            // Log the error message for debugging purposes
            Console.WriteLine(e.Message);
            // Return false to indicate that the authentication failed
            return false;
        }
    }
}
