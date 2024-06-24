using System.Windows.Input;
using ASiNet.App.RemoteKeyboard.Models;
using CommunityToolkit.Maui.Views;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class SavePresetQuest : Popup
{
	public SavePresetQuest(string? author = null, string? description = null, string? title = null)
	{
		InitializeComponent();
	    Author.Text = author;
        Description.Text = description;
        Title.Text = title;
    }

    public static readonly BindableProperty SaveCommandProperty =
            BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(SavePresetQuest));

    public ICommand? SaveCommand
    {
        get { return (ICommand)GetValue(SaveCommandProperty); }
        set { SetValue(SaveCommandProperty, value); }
    }


    private async void Save(object sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(Title.Text))
            return;
        SaveCommand?.Execute(new KeyboardPreset()
        { 
            Title = Title.Text,
            Description = Description.Text,
            Author = Author.Text
        });
        await CloseAsync();
    }

    private async void Cancel(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}