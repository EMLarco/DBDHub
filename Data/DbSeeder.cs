using Microsoft.EntityFrameworkCore;

namespace DBDHub.Data;

public static class DbSeeder
{
    private static readonly string[] SeedFiles =
    {
        Path.Combine("Data", "Seed", "dbd_seed.sql"),
        Path.Combine("Data", "Seed", "dbd_perks_seed.sql")
    };

    public static async Task ApplyAsync(AppDbContext context)
    {
        foreach (var relativePath in SeedFiles)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"No se encontro el script de seed: {fullPath}");
            }

            var sql = await File.ReadAllTextAsync(fullPath);
            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
