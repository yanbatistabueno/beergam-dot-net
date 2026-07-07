using Beergam.Services.User;

namespace Beergam.Services.Auth;

public interface IJwtService
{
    string GenerateToken(UserDTO.UserDto user);
}