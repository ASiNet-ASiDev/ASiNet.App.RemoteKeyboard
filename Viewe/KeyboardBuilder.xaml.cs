using System.Windows.Input;
using CommunityToolkit.Maui.Views;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class KeyboardBuilder : ContentPage
{
	public KeyboardBuilder()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty PresetTitleProperty =
            BindableProperty.Create(nameof(PresetTitle), typeof(string), typeof(KeyboardBuilder));

    public static readonly BindableProperty PresetAuthorProperty =
            BindableProperty.Create(nameof(PresetAuthor), typeof(string), typeof(KeyboardBuilder));

    public static readonly BindableProperty PresetDescriptionProperty =
            BindableProperty.Create(nameof(PresetDescription), typeof(string), typeof(KeyboardBuilder));

    public static readonly BindableProperty ClosePageCommandProperty =
            BindableProperty.Create(nameof(ClosePageCommand), typeof(ICommand), typeof(KeyboardBuilder));

    public string? PresetAuthor
    {
        get { return (string?)GetValue(PresetAuthorProperty); }
        set { SetValue(PresetAuthorProperty, value); }
    }

    public string? PresetTitle
    {
        get { return (string?)GetValue(PresetTitleProperty); }
        set { SetValue(PresetTitleProperty, value); }
    }

    public string? PresetDescription
    {
        get { return (string?)GetValue(PresetDescriptionProperty); }
        set { SetValue(PresetDescriptionProperty, value); }
    }

    public ICommand? ClosePageCommand
    {
        get { return (ICommand)GetValue(ClosePageCommandProperty); }
        set { SetValue(ClosePageCommandProperty, value); }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://fontawesome.com/search?q=edit&o=r&m=free");
    }

    private async void Save(object sender, EventArgs e)
    {
        var ksPopup = new SavePresetQuest(PresetAuthor, PresetDescription, PresetTitle) { BindingContext = BindingContext };
        var result = await this.ShowPopupAsync(ksPopup);
    }

    private async void Cancel(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    protected override void OnDisappearing()
    {
        ClosePageCommand?.Execute(null);
        base.OnDisappearing();
    }

    private async void OpenKeySettings(object sender, EventArgs e)
    {
        var ksPopup = new KeyOptions(this) { BindingContext = BindingContext };
        var result = await this.ShowPopupAsync(ksPopup);
    }
}