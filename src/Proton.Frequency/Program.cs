using Microsoft.Extensions.Options;
using Proton.Frequency.Services;
using Proton.Frequency.Services.ConfigOptions;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterConfigurations();
builder.RegisterHostOptions();
builder.RegisterStandardServices();
builder.Services.RegisterWorkersServices();

var app = builder.Build();
app.RegisterDefaults().RegisterEndpoints();

app.Run();
