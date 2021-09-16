using System.Threading.Tasks;
using NLog.Web;
using Iot.Rfid.Handlers;
using Iot.Rfid.Helpers;

var _config = new ConfigKey();
var _logger = new MainLogger();
var _readerProcess = new ReaderProcess();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();
/**
* Configure the HTTP request pipeline.
**/
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

System.Console.WriteLine(Figgle.FiggleFonts.Standard.Render("UHFRFID IOT"));
_logger.Trigger("Info", "Booting up daemon....");
/** Reader process thread **/
Task.Run(() => _readerProcess.Run()).Wait();

/** Web view thread **/
if (_config.BASE_WEB_ENABLE)
{
  Task.Run(() => app.Run()).Wait();
}
