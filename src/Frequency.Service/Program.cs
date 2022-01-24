using Proton.Frequency.Service;
using Proton.Frequency.Service.Process;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<StateWorker>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        // TODO: Handle file not fould exceptions..
        config
            .AddYamlFile("config.yaml", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .Build();

Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Proton . Frequency"));
await host.RunAsync();
