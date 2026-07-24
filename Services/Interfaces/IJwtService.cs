using cmkapi.Model;

namespace cmkapi.Services.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
}