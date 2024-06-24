using System.Windows.Input;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ImportCommandProperty =
            BindableProperty.Create(nameof(ImportCommand), typeof(ICommand), typeof(MainPage));

    public ICommand? ImportCommand
    {
        get { return (ICommand?)GetValue(ImportCommandProperty); }
        set { SetValue(ImportCommandProperty, value); }
    }

    private async void Import_Button_Pressed(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions() { PickerTitle = "Select Keyboard preset", });
        if(result != null)
        {
            ImportCommand?.Execute(result.FullPath);
        }
    }
}

