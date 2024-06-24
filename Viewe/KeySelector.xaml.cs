using ASiNet.App.RemoteKeyboard.VieweModels;
using CommunityToolkit.Maui.Views;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class KeySelector : Popup
{
	public KeySelector()
	{
		InitializeComponent();
	}

    private async void Button_Pressed(object sender, EventArgs e) => await CloseAsync(((KeySelectorVM)BindingContext).Selected);
}