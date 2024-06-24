using ASiNet.App.RemoteKeyboard.Models;
using ASiNet.RacAPI.Packages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ASiNet.App.RemoteKeyboard.VieweModels;
public partial class KeyboardPresenterVM(KeyboardPreset preset) : ObservableObject
{

    [ObservableProperty]
    private KeyboardPreset _preset = preset;


    private readonly object _locker = new();

    [RelayCommand]
    private async Task KeyPressed(KeyPresetVM key)
    {
        if (!App.RAClient.Authorized)
        {
            var cnf = Config.Read();
            if (!cnf.Autoconnect)
            {
                App.AlertService.ShowAlert("Disconnected", $"It looks like you are not connected to the computer and auto-connection is disabled");
            }
            else
            {
                using var cts = new CancellationTokenSource();
                cts.CancelAfter(5000);
                var result = await App.RAClient.Connect(cnf.Address, cnf.Port, cnf.Login, cnf.Password, RacAPI.Enums.RACPermissions.KeyboardAccess, cts.Token);

                var rtext = result switch
                {
                    RacAPI.Enums.RACResponseCode.Success => null,
                    RacAPI.Enums.RACResponseCode.IncorrectData => "Incorrect login or password.",
                    RacAPI.Enums.RACResponseCode.AccessDenied => $"On the server side, there is a restriction on connecting with these permissions: RemoteKeyboardAccess",
                    RacAPI.Enums.RACResponseCode.Timeout => "Time out.",
                    RacAPI.Enums.RACResponseCode.OutOfAttempts => "Out ot attempts count.",
                    RacAPI.Enums.RACResponseCode.Failed => "Failed.",
                    _ => "Failed",
                };
                if (rtext is not null)
                {
                    App.AlertService.ShowAlert("Connecting failed", rtext);
                    return;
                }
            }
        }
        KeyboardAccessRequest? request = null;
        switch (key.Preset.RequestType)
        {
            case KeyboardRequestType.SingleKey:
                request = KeyboardAccessRequest.SingleKey(key.Preset.KeyCode);
                break;
            case KeyboardRequestType.MultikeyOneModifier:
                request = KeyboardAccessRequest.MultikeyOneModifier(key.Preset.Mod1, key.Preset.KeyCode);
                break;
            case KeyboardRequestType.MultikeyTwoModifiers:
                request = KeyboardAccessRequest.MultikeyTwoModifiers(key.Preset.Mod1, key.Preset.Mod2, key.Preset.KeyCode);
                break;
        }
        if (request is not null)
        {
            if (!await App.RAClient.SendKeyboardEvent(request, default))
            {
                App.AlertService.ShowAlert("Oops :(", "The keyboard event could not be sent.");
            }
        }
        else
        {
            App.AlertService.ShowAlert("Oops :(", "It looks like this keyboard contains errors, the event was ignored.");
        }
    }
}
