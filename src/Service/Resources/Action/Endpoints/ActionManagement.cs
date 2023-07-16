using AutoMapper;
using Microsoft.Extensions.Options;
using Frequency.Common.Config;
using Frequency.Common.Dto;

namespace Frequency.Resources.Action.Endpoints;

public class ActionManagement {
    public ActionManagement(ILogger<ActionManagement> logger, IOptions<List<NetConfig>> config, IMapper mapper) {
        Logger = logger;
        Config = config.Value;
        Mapper = mapper;
    }

    private ILogger<ActionManagement> Logger { get; }
    private List<NetConfig> Config { get; }
    private IMapper Mapper { get; }

    public IResult Get() {
        Logger.LogInformation("------ {count} ------", Config.Count);
        return Results.Ok(Config.Select(test => Mapper.Map<SampleDto>(test)));
    }

    public IResult GetById(int id) {
        return Results.Ok(id);
    }
}