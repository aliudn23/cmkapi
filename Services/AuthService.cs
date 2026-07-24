using Microsoft.EntityFrameworkCore;
using cmkapi.DTO;
using cmkapi.Services.Interfaces;
using cmkapi.Data;
using cmkapi.Model;
using System.Security.Cryptography;

namespace cmkapi.Services;

public class AuthService(ApplicationDbContext context, IJwtService jwtService, IEmailService emailService, IConfiguration configuration) : IAuthService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IEmailService _emailService = emailService;
    private readonly IConfiguration _configuration = configuration;

    // Login Service
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        // Email Validate
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            throw new Exception("Email or Password is invalid");

        // Password Validate
        bool validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.Password
        );

        if (!validPassword)
            throw new Exception("Email or Password is invalid");

        // Generate Token
        var accessToken = _jwtService.GenerateAccessToken(user);

        // Update Last Login
        user.LastLogin = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            AccessToken = accessToken,
            ExpiredAt = DateTime.UtcNow.AddDays(7),
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };
    }

    // Register Service
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        // Check Email is exists
        var emailExists = await _context.Users
        .AnyAsync(x => x.Email == request.Email);

        if (emailExists)
            throw new Exception("Email has been registered");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);

        // Do Save User
        await _context.SaveChangesAsync();

        return new RegisterResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        // Cek User
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            return;

        // Generate Token
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

        // Save Token
        _context.PasswordResetTokens.Add(
            new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30),
                CreatedAt = DateTime.UtcNow
            }
        );

        await _context.SaveChangesAsync();

        var host = _configuration["Client:Host"];
        var port = _configuration["Client:Port"];

        // Sending Email
        await _emailService.SendAsync(
            user.Email,
            "Reset Password",
            $@"
            <h2>Reset Password</h2>

            <p>Klik link berikut untuk reset password.</p>

            <a href='{host}:{port}/reset-password?token={token}'>
                Reset Password
            </a>"
        );
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        // Check token
        var reset = await _context.PasswordResetTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == request.Token && x.UsedAt == null);

        if (reset == null)
            throw new Exception("Token Expired");

        // Do Reset Password
        reset.User.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        reset.User.UpdatedAt = DateTime.UtcNow;

        // Flag as used
        reset.UsedAt = DateTime.UtcNow;

        // Do Reset
        await _context.SaveChangesAsync();
    }
}