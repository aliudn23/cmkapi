using cmkapi.DTO;

namespace cmkapi.Services.Interfaces;

public interface IAuthService
{
    // Login
    Task<LoginResponse> LoginAsync(LoginRequest request);

    // Register
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);

    // Forgot Password
    Task ForgotPasswordAsync(ForgotPasswordRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
}