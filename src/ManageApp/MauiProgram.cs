using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using UraniumUI;

namespace Proton.Frequency.ManageApp {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts => {
                    fonts.AddFontAwesomeIconFonts();
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<App>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}