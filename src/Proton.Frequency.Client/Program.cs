using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Proton.Frequency.Client;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
#if DEBUG
builder.Services.AddSassCompiler();
#endif
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient("Proton.Frequency", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
await builder.Build().RunAsync();
