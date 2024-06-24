using CommunityToolkit.Maui;
using ASiNet.Yandex.Ads;
using Microsoft.Extensions.Logging;

namespace ASiNet.App.RemoteKeyboard;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.RegisterYandexAds();
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Awesome6-Solid.otf", "A6");
                fonts.AddFont("Font Awesome 6 Brands.otf", "A6-B");
            });
        builder.Services.AddSingleton<IAlertService, AlertService>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
