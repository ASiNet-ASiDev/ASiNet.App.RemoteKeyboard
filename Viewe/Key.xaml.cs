using ASiNet.App.RemoteKeyboard.VieweModels;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class Key : Button
{
    public Key(KeyPresetVM preset)
    {
        Preset = preset;
        InitializeComponent();
        BindingContext = preset;
    }

    public KeyPresetVM Preset { get; init; }
}