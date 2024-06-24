
using ASiNet.App.RemoteKeyboard.Models;
using ASiNet.App.RemoteKeyboard.Resources.Langs;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        var cnf = Config.Read();
        Address.Text = cnf.Address;
        Port.Text = cnf.Port.ToString();
        Login.Text = cnf.Login;
        Password.Text = cnf.Password;
        AutoConnect.IsChecked = cnf.Autoconnect;
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        var port = Config.Default.Port;
        int.TryParse(Port.Text, out port);

        var cnf = new Config()
        {
             Address = Address.Text,
             Port = port,
             Login = string.IsNullOrWhiteSpace(Login.Text) ? null : Login.Text,
             Password = string.IsNullOrWhiteSpace(Password.Text) ? null : Password.Text,
             Autoconnect = AutoConnect.IsChecked,
        };
        Config.Write(cnf);
        base.OnDisappearing();
    }

    private async void Button_Pressed(object sender, EventArgs e)
    {
        try
        {
            ConnectingResult.Text = AppResources.SettingsConnectResultConnecting;
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            var login = string.IsNullOrWhiteSpace(Login.Text) ? null : Login.Text;
            var password = string.IsNullOrWhiteSpace(Password.Text) ? null : Password.Text;
            var result = await App.RAClient.Connect(Address.Text, int.Parse(Port.Text), login, password, RacAPI.Enums.RACPermissions.KeyboardAccess, cts.Token);

            ConnectingResult.Text = result switch
            {
                RacAPI.Enums.RACResponseCode.Success => AppResources.SettingsConnectResultSuccess,
                RacAPI.Enums.RACResponseCode.IncorrectData => AppResources.SettingsConnectResultIncorrectData,
                RacAPI.Enums.RACResponseCode.AccessDenied => AppResources.SettingsConnectResultAccessDenied,
                RacAPI.Enums.RACResponseCode.Timeout => AppResources.SettingsConnectResultTimeout,
                RacAPI.Enums.RACResponseCode.OutOfAttempts => AppResources.SettingsConnectResultOutOfAttempts,
                RacAPI.Enums.RACResponseCode.Failed => AppResources.SettingsConnectResultFailed,
                _ => AppResources.SettingsConnectResultFailed,
            };
        }
        catch (Exception ex)
        {
            ConnectingResult.Text = ex.Message;
        }
    }

    private async void Telegram_Tapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://t.me/asinet_projects");
    }

    private async void Website_Tapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://asinet-asidev.github.io/");
    }

    private async void YouTube_Tapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://www.youtube.com/channel/UCk5N0UbHCPc8nfmKY-aKMMQ");
    }
}