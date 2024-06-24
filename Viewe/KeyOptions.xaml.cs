using System.Windows.Input;
using CommunityToolkit.Maui.Views;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class KeyOptions : Popup
{
	public KeyOptions(Page parent)
	{
        _parent = parent;
		InitializeComponent();
	}

    public static readonly BindableProperty SelectedMod1CommandProperty =
            BindableProperty.Create(nameof(SelectedMod1Command), typeof(ICommand), typeof(KeyboardBuilder));

    public static readonly BindableProperty SelectedMod2CommandProperty =
            BindableProperty.Create(nameof(SelectedMod2Command), typeof(ICommand), typeof(KeyboardBuilder));

    public static readonly BindableProperty SelectedKeyCodeCommandProperty =
            BindableProperty.Create(nameof(SelectedKeyCodeCommand), typeof(ICommand), typeof(KeyboardBuilder));

    private Page _parent;

    public ICommand? SelectedMod2Command
    {
        get { return (ICommand)GetValue(SelectedMod2CommandProperty); }
        set { SetValue(SelectedMod2CommandProperty, value); }
    }

    public ICommand? SelectedKeyCodeCommand
    {
        get { return (ICommand)GetValue(SelectedKeyCodeCommandProperty); }
        set { SetValue(SelectedKeyCodeCommandProperty, value); }
    }

    public ICommand? SelectedMod1Command
    {
        get { return (ICommand)GetValue(SelectedMod1CommandProperty); }
        set { SetValue(SelectedMod1CommandProperty, value); }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://fontawesome.com/search?o=r&m=free");
    }

    private async void SelectMod1(object sender, EventArgs e)
    {
        var ksPopup = new KeySelector();
        var result = await _parent.ShowPopupAsync(ksPopup);
        SelectedMod1Command?.Execute(result);
    }
    private async void SelectMod2(object sender, EventArgs e)
    {
        var ksPopup = new KeySelector();
        var result = await _parent.ShowPopupAsync(ksPopup);
        SelectedMod2Command?.Execute(result);
    }
    private async void SelectKeyCode(object sender, EventArgs e)
    {
        var ksPopup = new KeySelector();
        var result = await _parent.ShowPopupAsync(ksPopup);
        SelectedKeyCodeCommand?.Execute(result);
    }
}