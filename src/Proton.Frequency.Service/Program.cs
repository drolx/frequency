using Proton.Frequency.Service.Handlers;
using Proton.Frequency.Service.Helpers;

var _config = new ConfigKey();
var _logger = new MainLogger();
var _readerProcess = new ReaderProcess();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

System.Console.WriteLine(Figgle.FiggleFonts.Standard.Render("UHF RFID IOT"));
_logger.Trigger("Info", "Booting up daemon....");
Task.Run(() => _readerProcess.Run()).Wait();

if (_config.BASE_WEB_ENABLE)
{
    Task.Run(() => app.Run()).Wait();
}
