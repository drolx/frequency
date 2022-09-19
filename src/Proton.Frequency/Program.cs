var builder = WebApplication.CreateBuilder(args);
var configFiles = new [] { "main", "serial", "network" };
foreach (var configFile in configFiles)
{
    builder.Configuration.AddYamlFile($"config.{configFile}.yaml", optional: false, reloadOnChange: true);
}
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
app.Run();
