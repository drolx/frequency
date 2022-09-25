var builder = WebApplication.CreateBuilder(args);
var configs = new List<string>() { "main", "serial", "network" };
configs.ForEach(
    n =>
        builder.Configuration.AddYamlFile($"config.{n}.yaml", optional: false, reloadOnChange: true)
);
builder.Host.UseSystemd();
builder.Services.AddRazorPages();
#if DEBUG
builder.Services.AddSassCompiler();
#endif

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
app.Run();
