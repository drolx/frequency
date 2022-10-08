using AutoMapper;
using Microsoft.Extensions.Options;
using Proton.Frequency.Common.Dto;
using Proton.Frequency.Config;

namespace Proton.Frequency.Module.Action.Endpoints;

public class ActionManagement
{
    private ILogger<ActionManagement> Logger { get; }
    private List<NetworkConfig> Config { get; }
    private IMapper Mapper { get; }
    
    public ActionManagement(ILogger<ActionManagement> logger, IOptions<List<NetworkConfig>> config, IMapper mapper)
    {
        Logger = logger;
        Config = config.Value;
        Mapper = mapper;
    }

    public IResult Get() {
        Logger.LogInformation("------ {count} ------", Config.Count);
        return Results.Ok(Config.Select(test => Mapper.Map<SampleDto>(test)));
    }

    public IResult GetById(int id)
    {
        return Results.Ok(id);
    }
}
