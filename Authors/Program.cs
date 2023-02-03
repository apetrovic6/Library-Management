using System.Reflection;
using Authors.Data;
using Authors.Services;
using Authors.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddPooledDbContextFactory<AuthorsDbContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthorService>();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    await Task.Delay(5000);
    
    try
    {
        var contextFactory = services.GetRequiredService<IDbContextFactory<AuthorsDbContext>>();

        await using var context = await contextFactory.CreateDbContextAsync();
        await context.Database.MigrateAsync();
        await InitDB.Init(context, loggerFactory);

        await context.DisposeAsync();
    }
    catch (Exception e)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(e, "An Error occured during migration");
    }
}

app.Run();