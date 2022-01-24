using Proton.Frequency.Service;
using Proton.Frequency.Service.Process;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting.Systemd;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .UseSystemd()
    .ConfigureServices(services =>
    {
        services.AddHostedService<StateWorker>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        // TODO: Handle file not fould exceptions..
        config
            .AddYamlFile("defaults.yaml", optional: false, reloadOnChange: true)
            .AddYamlFile("channels.yaml", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .Build();

Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Proton . Frequency"));
await host.RunAsync();
