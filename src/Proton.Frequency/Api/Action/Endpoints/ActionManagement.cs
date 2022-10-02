using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Proton.Frequency.Config;

namespace Proton.Frequency.Api.Action.Endpoints; 

public class ActionManagement {
    private ILogger<ActionManagement> _logger { get; set; }
    private IOptions<DefaultConfig> _config { get; set; }
    
    public ActionManagement(ILogger<ActionManagement> logger, IOptions<DefaultConfig> config) {
        _logger = logger;
        _config = config;
    }
    
    public IResult Get()
    {
        return Results.Ok("Action");
    }
    
    public IResult GetById(int id)
    {
        return Results.Ok(id);
    }

}
