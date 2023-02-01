using System.Reflection;
using Books.Data;
using Books.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddPooledDbContextFactory<BooksDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();


// Configure the GRPC request pipeline.
app.MapGrpcService<BookService>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    await Task.Delay(5000);
    try
    {
        var contextFactory = services.GetRequiredService<IDbContextFactory<BooksDbContext>>();
        
        using(var context = contextFactory.CreateDbContext()){
            await context.Database.MigrateAsync();
            await InitDB.Init(context, loggerFactory);
        }

    }
    catch (Exception e)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(e, "An error occured during migration");
    }
}

app.Run();