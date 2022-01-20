var builder = WebApplication.CreateBuilder(args);
var startUpText = Figgle.FiggleFonts.Standard.Render("Proton . Frequency");

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
app.Logger.LogInformation(startUpText);
app.Run();
