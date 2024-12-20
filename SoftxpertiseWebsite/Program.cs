using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Softxpertise.Website;
using Softxpertise.Website.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<ContactService>("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://contact-api-ccg7e8hbdxe6d5dz.westeurope-01.azurewebsites.net/"); // URL de l'API Azure
});

await builder.Build().RunAsync();
