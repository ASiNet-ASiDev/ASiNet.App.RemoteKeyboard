﻿namespace ASiNet.App.RemoteKeyboard;

public interface IAlertService
{
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
    Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No");


    void ShowAlert(string title, string message, string cancel = "OK");

    void ShowConfirmation(string title, string message, Action<bool> callback,
                          string accept = "Yes", string cancel = "No");

    Task<string> ShowActionSheet(string title, string? cancel, string? destruct, params string[] buttons);
}

internal class AlertService : IAlertService
{

    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        return Application.Current!.MainPage!.DisplayAlert(title, message, cancel);
    }

    public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        return Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
    }

    public void ShowAlert(string title, string message, string cancel = "OK")
    {
        Application.Current!.MainPage!.Dispatcher.Dispatch(async () =>
            await ShowAlertAsync(title, message, cancel)
        );
    }

    public void ShowConfirmation(string title, string message, Action<bool> callback,
                                 string accept = "Yes", string cancel = "No")
    {
        Application.Current!.MainPage!.Dispatcher.Dispatch(async () =>
        {
            bool answer = await ShowConfirmationAsync(title, message, accept, cancel);
            callback(answer);
        });
    }

    public Task<string> ShowActionSheet(string title, string? cancel, string? destruct, params string[] buttons)
    {
        return Application.Current!.MainPage!.DisplayActionSheet(title, cancel, destruct, buttons);
    }
}
