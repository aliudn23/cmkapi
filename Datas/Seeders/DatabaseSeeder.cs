using cmkapi.Data;
using cmkapi.Seeders;

namespace cmkapi.Data.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await UserSeeder.SeedAsync(context);
    }
}