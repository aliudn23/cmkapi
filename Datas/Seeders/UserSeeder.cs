using cmkapi.Data;
using cmkapi.Model;
using Microsoft.EntityFrameworkCore;

namespace cmkapi.Seeders;

public static class UserSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Cek apakah admin sudah ada
        if (await context.Users.AnyAsync(x => x.Email == "admin@cmk.test"))
            return;

        var admin = new User
        {
            Name = "Administrator",
            Email = "admin@cmk.test",
            Password = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}