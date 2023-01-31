using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using WebClient;
using WebClient.DTO;
using WebClient.Services;
using WebClient.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMudServices();
builder.Services.AddScoped<IGenericService<Book>, BookService>();

builder.Services.AddBooksClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://acme.com/graphql"));

await builder.Build().RunAsync();