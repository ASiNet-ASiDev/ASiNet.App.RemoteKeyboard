using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASiNet.App.RemoteKeyboard.Models;
using ASiNet.App.RemoteKeyboard.VieweModels;

namespace ASiNet.App.RemoteKeyboard.Viewe;

public partial class KeyboardControl : Grid
{
    public KeyboardControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty KeyCommandProperty =
            BindableProperty.Create(nameof(KeyCommand), typeof(ICommand), typeof(KeyboardControl));

    public static readonly BindableProperty KeyboardPresetProperty =
            BindableProperty.Create(nameof(KeyboardPreset), typeof(KeyboardPreset), typeof(KeyboardControl));

    public static readonly BindableProperty KeysProperty =
            BindableProperty.Create(nameof(Keys), typeof(ObservableCollection<KeyPresetVM>), typeof(KeyboardControl));

    public ICommand? KeyCommand
    {
        get { return (ICommand?)GetValue(KeyCommandProperty); }
        set { SetValue(KeyCommandProperty, value); }
    }

    public KeyboardPreset? KeyboardPreset
    {
        get { return (KeyboardPreset?)GetValue(KeyboardPresetProperty); }
        set { SetValue(KeyboardPresetProperty, value); }
    }

    public ObservableCollection<KeyPresetVM>? Keys
    {
        get { return (ObservableCollection<KeyPresetVM>?)GetValue(KeysProperty); }
        set { SetValue(KeysProperty, value); }
    }

    public void AddButton(KeyPresetVM preset)
    {
        var button = new Key(preset);
        button.Pressed += OnKeyPressed;
        Root.AddWithSpan(button, preset.Preset.PosY, preset.Preset.PosX, preset.Preset.SizeY, preset.Preset.SizeX);
    }

    public void RemoveButton(KeyPresetVM preset)
    {
        var child = Root.Children.FirstOrDefault(x => (x as Key)?.Preset.Id == preset.Id);
        if (child != null)
        {
            (child as Key)!.Pressed -= OnKeyPressed;
            Root.Children.Remove(child);
        }
    }

    private void OnKeyPressed(object? sender, EventArgs e)
    {
        KeyCommand?.Execute((sender as Key)?.Preset);
    }


    public void EditButton(KeyPresetVM preset)
    {
        var child = Root.Children.FirstOrDefault(x => (x as Key)?.Preset.Id == preset.Id);
        if (child != null)
        {
            Root.SetColumn(child, preset.Preset.PosX);
            Root.SetRow(child, preset.Preset.PosY);
            Root.SetColumnSpan(child, preset.Preset.SizeX);
            Root.SetRowSpan(child, preset.Preset.SizeY);
        }
    }

    public void ClearButtons()
    {
        Root.Children.Clear();
    }

    public void SetKeyboardPreset(KeyboardPreset preset)
    {
        if (preset.Keys is null)
            return;
        foreach (var keyPreset in preset.Keys)
        {
            App.Current!.Dispatcher.Dispatch(() => AddButton(new(keyPreset)));
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (propertyName == nameof(KeyboardPreset))
        {
            ClearButtons();
            if (KeyboardPreset != null)
            {
                SetKeyboardPreset(KeyboardPreset);
            }
        }
        else if(propertyName == nameof(Keys))
        {
            if(Keys is not null)
            {
                App.Current!.Dispatcher.Dispatch(ClearButtons);
                foreach (var keyPreset in Keys)
                {
                    App.Current!.Dispatcher.Dispatch(() => AddButton(keyPreset));
                }
                Keys.CollectionChanged += OnKeysChandged;
            }
        }
        base.OnPropertyChanged(propertyName);
    }

    private void OnKeysChandged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddButton((KeyPresetVM)e.NewItems![0]!);
                break;
            case NotifyCollectionChangedAction.Remove:
                RemoveButton((KeyPresetVM)e.OldItems![0]!);
                break;
            case NotifyCollectionChangedAction.Replace:
                EditButton((KeyPresetVM)e.NewItems![0]!);
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                break;
        }
    }
}