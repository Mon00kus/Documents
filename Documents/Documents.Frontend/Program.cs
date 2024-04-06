using Documents.Frontend;
using Documents.Frontend.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// Registra HttpClient como un servicio
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7147/") });

builder.Services.AddScoped<IRepository, Repository>();

await builder.Build().RunAsync();
