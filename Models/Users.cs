namespace cmkapi.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    public ICollection<RefreshToken> refreshTokens { get; set; } = [];
}

public class PasswordResetToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = "";
    public DateTime ExpiredAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; } = null!;
}


public class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    // SHA256 Hash
    public string TokenHash { get; set; } = "";

    public string DeviceName { get; set; } = "";

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}