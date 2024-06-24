using ASiNet.App.RemoteKeyboard.Models;
using ASiNet.RacAPI;

namespace ASiNet.App.RemoteKeyboard;

public partial class App : Application
{
    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Services = provider;
        AlertService = Services.GetService<IAlertService>()!;
        RAClient = new RAClient();
        MainPage = new AppShell();
    }


    public static IServiceProvider Services { get; private set; } = null!;
    public static IAlertService AlertService { get; private set; } = null!;

    public static KeyboardsManager KeyboardsManager => _kp.Value;

    public static RAClient RAClient {  get; private set; } = null!;

    private static Lazy<KeyboardsManager> _kp = new(() => new());
}
