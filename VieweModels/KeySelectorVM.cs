using System.ComponentModel;
using ASiNet.App.RemoteKeyboard.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ASiNet.App.RemoteKeyboard.VieweModels;
public partial class KeySelectorVM : ObservableObject
{
    public KeySelectorVM()
    {

    }

    [ObservableProperty]
    private string[] _keys = TotalKeysCollection.KeyNames;

    [ObservableProperty]
    private string? _selected;

    [ObservableProperty]
    private string? _filter;

    private readonly object _locker = new();

    private async Task UpdateKeys(string? filters)
    {
        await Task.Run(() =>
        {
            lock (_locker)
            {
                Keys = [.. TotalKeysCollection.EnumerateKeys(filters)];
            }
        });
    }


    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Filter))
        {
            _ = UpdateKeys(Filter);
        }
        base.OnPropertyChanged(e);
    }
}
