using Proton.Frequency;
using Proton.Frequency.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterConfigurations();
builder.RegisterHostOptions();
builder.RegisterStandardServices();
builder.Services.RegisterWorkersServices();

var app = builder.Build();
app.RegisterDefaults().RegisterEndpoints();

app.Run();
