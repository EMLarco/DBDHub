using DBDHub.Data;
using DBDHub.Hubs;
using DBDHub.Models;
using DBDHub.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Registrar el DbContext con PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Requisitos de contraseña relajados para desarrollo
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configurar la cookie de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Comunicación en tiempo real (SignalR). Si hay Redis configurado se activa
// el backplane para repartir los eventos entre todas las réplicas.
var signalR = builder.Services.AddSignalR();
var redisConnection = builder.Configuration.GetConnectionString("Redis");
var dataProtection = builder.Services.AddDataProtection()
    .SetApplicationName("DBDHub");

if (!string.IsNullOrWhiteSpace(redisConnection))
{
    signalR.AddStackExchangeRedis(redisConnection, options =>
    {
        options.Configuration.AbortOnConnectFail = false;
    });

    // Claves compartidas para que cookies y tokens antiforgery funcionen
    // detras de varias replicas.
    var redisOptions = ConfigurationOptions.Parse(redisConnection);
    redisOptions.AbortOnConnectFail = false;
    var redis = await ConnectionMultiplexer.ConnectAsync(redisOptions);
    builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
    dataProtection.PersistKeysToStackExchangeRedis(redis, "DBDHub-DataProtection-Keys");
}

builder.Services.AddSingleton<ILiveEventPublisher, LiveEventPublisher>();

var app = builder.Build();

// Modo mantenimiento para Kubernetes: aplicar migraciones y/o cargar datos.
var forceSeed = args.Contains("--force-seed");
if (args.Contains("--migrate") || args.Contains("--seed") || forceSeed)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();

        if (args.Contains("--seed") || forceSeed)
        {
            var alreadySeeded = await db.Killers.AnyAsync() && await db.Perks.AnyAsync();
            if (forceSeed || !alreadySeeded)
            {
                await DbSeeder.ApplyAsync(db);
            }
            else
            {
                Console.WriteLine("El catalogo ya esta sembrado. Usa --force-seed para regenerarlo.");
            }
        }
    }

    return;
}

// Cabeceras de seguridad aplicadas a todas las respuestas
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self'; " +
        "style-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com https://fonts.googleapis.com; " +
        "font-src 'self' data: https://cdnjs.cloudflare.com https://fonts.gstatic.com; " +
        "img-src 'self' data: https: http:; " +
        "connect-src 'self' ws: wss:; " +
        "object-src 'none'; " +
        "base-uri 'self'; " +
        "form-action 'self'; " +
        "frame-ancestors 'none'";
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

// Añadir autenticación antes de autorización
app.UseAuthentication();
app.UseAuthorization();

// Endpoints de salud usados por los probes de Kubernetes.
app.MapGet("/health/live", () => Results.Ok(new
{
    status = "alive",
    server = Environment.GetEnvironmentVariable("HOSTNAME") ?? Environment.MachineName
}));

app.MapGet("/health/ready", async (AppDbContext db) =>
{
    try
    {
        if (!await db.Database.CanConnectAsync())
        {
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        var pending = await db.Database.GetPendingMigrationsAsync();
        if (pending.Any())
        {
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        var catalogReady = await db.Killers.AnyAsync() && await db.Perks.AnyAsync();
        return catalogReady
            ? Results.Ok(new { status = "ready" })
            : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
    catch
    {
        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
});

app.MapHub<LiveHub>("/hubs/live");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
