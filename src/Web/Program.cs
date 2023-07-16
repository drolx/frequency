using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frequency.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
#if DEBUG
builder.Services.AddSassCompiler();
#endif
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services
    .AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();
builder.Services.AddHttpClient(
    "Frequency",
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
);
builder.Services.AddScoped(
    sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);
await builder.Build().RunAsync();