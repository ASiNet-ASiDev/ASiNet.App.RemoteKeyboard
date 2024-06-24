using System.Collections.ObjectModel;
using System.ComponentModel;
using ASiNet.App.RemoteKeyboard.Models;
using ASiNet.App.RemoteKeyboard.Viewe;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ASiNet.App.RemoteKeyboard.VieweModels;
public partial class MainPageVM : ObservableObject
{
    public MainPageVM()
    {
        _ = LoadKeyboards();
    }

    public ObservableCollection<KeyboardPreset> Keyboards { get; } = [];

    [ObservableProperty]
    private KeyboardPreset? _selected;

    [ObservableProperty]
    private bool _isSelected;

    [RelayCommand]
    private async Task Add()
    {
        await Shell.Current.Navigation.PushModalAsync(new KeyboardBuilder() { BindingContext = new KeyboardBuilderVM() });
    }

    [RelayCommand]
    private async Task Edit()
    {
        await Shell.Current.Navigation.PushModalAsync(new KeyboardBuilder() { BindingContext = new KeyboardBuilderVM(Selected!) });
    }

    [RelayCommand]
    private async Task Remove()
    {
        if(await App.KeyboardsManager.Remove(Selected!))
        {
            Keyboards.Remove(Selected!);
            Selected = null;
        }
    }

    [RelayCommand]
    private async Task Show()
    {
        await Shell.Current.Navigation.PushModalAsync(new KeyboardPresenter() { BindingContext = new KeyboardPresenterVM(Selected!) });
    }

    [RelayCommand]
    private async Task Settings()
    {
        await Shell.Current.Navigation.PushModalAsync(new Settings());
    }

    [RelayCommand]
    private async Task Refresh() =>
        await LoadKeyboards();

    [RelayCommand]
    private async Task Import(string path)
    {
        if(await App.KeyboardsManager.ImportPreset(path))
        {
            await Refresh();
        }
        else
        {
            await App.AlertService.ShowAlertAsync("The file could not be imported", "The application may not have access to the directory");
        }
    }

    [RelayCommand]
    private async Task Export()
    {
        await App.KeyboardsManager.ExportPreset(Selected!);
    }

    private async Task LoadKeyboards()
    {
        await Task.Run(() =>
        {
            Keyboards.Clear();
            foreach (var keyboard in App.KeyboardsManager.EnumerateKeyboards())
            {
                if(keyboard is null)
                    continue;
                Keyboards.Add(keyboard);
            }
        });
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if(e.PropertyName == nameof(Selected))
        {
            if(Selected != null)
                IsSelected = true;
            else IsSelected = false;
        }

        base.OnPropertyChanged(e);
    }
}
