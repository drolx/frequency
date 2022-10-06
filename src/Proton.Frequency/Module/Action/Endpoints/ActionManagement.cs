using Microsoft.Extensions.Options;
using Proton.Frequency.Config;

namespace Proton.Frequency.Module.Action.Endpoints;

public class ActionManagement
{
    public ActionManagement(ILogger<ActionManagement> logger, IOptions<List<NetworkConfig>> config)
    {
        Logger = logger;
        Config = config.Value;
    }

    private ILogger<ActionManagement> Logger { get; }
    private List<NetworkConfig> Config { get; }

    public IResult Get()
    {
        Logger.LogInformation("-------------------------- {name} ---------------", Config.Count);
        return Results.Ok(Config);
    }

    public IResult GetById(int id)
    {
        return Results.Ok(id);
    }
}
