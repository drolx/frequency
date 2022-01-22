using Proton.Frequency.Service;

var startUpText = Figgle.FiggleFonts.Standard.Render("Proton . Frequency");
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ServiceWorker>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .AddYamlFile("config.yaml", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .Build();

Console.WriteLine(startUpText);
await host.RunAsync();
