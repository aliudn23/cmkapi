using Microsoft.AspNetCore.Mvc;
using cmkapi.DTO;
using cmkapi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace cmkapi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // development
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(1)
        };

        Response.Cookies.Append(
            "access_token",
            result.AccessToken,
            cookieOptions
        );

        Response.Cookies.Append(
            "refresh_token",
            result.RefreshToken,
            cookieOptions
        );

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        await _authService.ForgotPasswordAsync(request);

        return Ok(new { success = true, message = "We have sent you an email, please check it on your inbox" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        await _authService.ResetPasswordAsync(request);

        return Ok(new { success = true, message = "Password has been changed" });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access_token");

        return Ok(new { success = true, message = "Logout Successfuly"});
    }
}