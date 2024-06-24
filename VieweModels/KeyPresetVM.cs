using System.ComponentModel;
using ASiNet.App.RemoteKeyboard.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ASiNet.App.RemoteKeyboard.VieweModels;

public partial class KeyPresetVM(KeyPreset preset) : ObservableObject
{
    [ObservableProperty]
    private Guid _id = Guid.NewGuid();
    [ObservableProperty]
    private KeyPreset _preset = preset;
    [ObservableProperty]
    private string? _visualText = preset.VisualText;


    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(VisualText))
        {
            Preset.VisualText = VisualText;
        }
        base.OnPropertyChanged(e);
    }
}
