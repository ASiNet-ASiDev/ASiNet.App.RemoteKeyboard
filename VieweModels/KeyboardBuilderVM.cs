using System.Collections.ObjectModel;
using System.ComponentModel;
using ASiNet.App.RemoteKeyboard.Models;
using ASiNet.App.RemoteKeyboard.Resources.Langs;
using ASiNet.RacAPI.Packages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ASiNet.App.RemoteKeyboard.VieweModels;
public partial class KeyboardBuilderVM : ObservableObject
{
    public KeyboardBuilderVM()
    {
        Keys = [];
        _ = LoadBackup();
    }

    public KeyboardBuilderVM(KeyboardPreset preset)
    {
        Author = preset.Author;
        Title = preset.Title;
        Description = preset.Description;
        Keys = [];
        if (preset.Keys is not null)
            Load(preset);
    }

    public ObservableCollection<KeyPresetVM> Keys { get; }

    [ObservableProperty]
    private KeyPresetVM? _selectedKey;

    [ObservableProperty]
    private bool _allowModify;

    [ObservableProperty]
    private bool _isSingleKey;
    [ObservableProperty]
    private bool _isMultiKey2;
    [ObservableProperty]
    private bool _isMultiKey3;

    [ObservableProperty]
    private KeyboardRequestType _RequestType;

    [ObservableProperty]
    private string? _selectedKeyName;

    [ObservableProperty]
    private string? _mod1;

    [ObservableProperty]
    private string? _mod2;

    [ObservableProperty]
    private string? _keyCode;

    [ObservableProperty]
    private string? _author;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private string? _title;

    private int _indexer = 0;

    [RelayCommand]
    private async Task SaveKeyboard(KeyboardPreset preset)
    {
        preset.Keys = Keys.Select(x => x.Preset).ToArray();
        if (!await App.KeyboardsManager.SaveOrUpdate(preset))
        {
            App.AlertService.ShowAlert(AppResources.BuilderSaveMessageErrorTitle, AppResources.BuilderSaveMessageErrorDescription);
        }
        else
        {
            App.AlertService.ShowAlert(AppResources.BuilderSaveMessageOkTitle, AppResources.BuilderSaveMessageOkDescription);
        }
    }

    [RelayCommand]
    private async Task LoadBackup()
    {
        if(App.KeyboardsManager.LoadBackup() is KeyboardPreset bk)
        {
            if(await App.AlertService.ShowConfirmationAsync(AppResources.BuilderLoadPrevSessionTitle, AppResources.BuilderLoadPrevSessionDescription))
            {
                Author = bk.Author;
                Title = bk.Title;
                Description = bk.Description;
                if (bk.Keys is not null)
                    Load(bk);
            }
        }
    }

    [RelayCommand]
    private async Task CreateBackup()
    {
        if(Keys.Count == 0)
            return;
        var preset = new KeyboardPreset
        {
            Author = Author,
            Title = Title ?? DateTime.Now.ToString("g"),
            Description = Description,
            Keys = Keys.Select(x => x.Preset).ToArray()
        };
        if (!await App.KeyboardsManager.CreateBackup(preset))
        {
            App.AlertService.ShowAlert(AppResources.BuilderCreateBackupErrorTitle, AppResources.BuilderCreateBackupErrorDescription);
        }
    }

    [RelayCommand]
    private void SelectKey(KeyPresetVM key)
    {
        SelectedKey = key;
    }

    [RelayCommand]
    private void UnsellectButton()
    {
        SelectedKey = null;
    }

    [RelayCommand]
    private void AddButton()
    {
        _indexer++;
        var newKey = new KeyPresetVM(new KeyPreset() { VisualText = _indexer.ToString(), PosX = 6, PosY = 6 });
        Keys.Add(newKey);
        SelectedKey = newKey;
    }

    [RelayCommand]
    private void RemoveButton()
    {
        Keys.Remove(SelectedKey!);
        SelectedKey = null;
    }

    [RelayCommand]
    private void SetPosXY(string coord)
    {
        if (SelectedKey is null)
            return;
        var p = coord.Split(':');
        SelectedKey!.Preset.PosX = int.Parse(p.First());
        SelectedKey!.Preset.PosY = int.Parse(p.Last());
        Keys[Keys.IndexOf(SelectedKey)] = SelectedKey;
    }

    [RelayCommand]
    private async Task SetKeyMode()
    {
        var res = await App.AlertService.ShowActionSheet(AppResources.SelectKeyModeTitle, "cancel", null,
            AppResources.SelectSingleKey,
            AppResources.SelectMultiKey1,
            AppResources.SelectMultiKey2);
        if(res ==  AppResources.SelectSingleKey)
            SetKeyModeBase(KeyboardRequestType.SingleKey);
        else if(res == AppResources.SelectMultiKey1)
            SetKeyModeBase(KeyboardRequestType.MultikeyOneModifier);
        else if (res == AppResources.SelectMultiKey2)
            SetKeyModeBase(KeyboardRequestType.MultikeyTwoModifiers);
    }

    [RelayCommand]
    private void MoveX(int offset)
    {
        if ((SelectedKey!.Preset.PosX == 0 && offset == -1) ||
            (SelectedKey!.Preset.PosX >= 12 && offset == 1))
            return;
        SelectedKey!.Preset.PosX += offset;
        Keys[Keys.IndexOf(SelectedKey)] = SelectedKey;
    }
    [RelayCommand]
    private void MoveY(int offset)
    {
        if ((SelectedKey!.Preset.PosY == 0 && offset == -1) ||
            (SelectedKey!.Preset.PosY >= 12 && offset == 1))
            return;
        SelectedKey!.Preset.PosY += offset;
        Keys[Keys.IndexOf(SelectedKey)] = SelectedKey;
    }
    [RelayCommand]
    private async Task SetSizeX()
    {
        var res = await App.AlertService.ShowActionSheet(AppResources.SetSizeXTitle, "cancel", null,
            AppResources.SelectSmallSize,
            AppResources.SelectMediumSize,
            AppResources.SelectLargeSize);
        var r = SizePresetToSize(res);
        if (r is null)
            return;
        SelectedKey!.Preset.SizeX = r.Value;
        Keys[Keys.IndexOf(SelectedKey)] = SelectedKey;
    }
    [RelayCommand]
    private async Task SetSizeY()
    {
        var res = await App.AlertService.ShowActionSheet(AppResources.SetSizeYTitle, "cancel", null,
            AppResources.SelectSmallSize,
            AppResources.SelectMediumSize,
            AppResources.SelectLargeSize);
        var r = SizePresetToSize(res);
        if(r is null)
            return;
        SelectedKey!.Preset.SizeY = r.Value;
        Keys[Keys.IndexOf(SelectedKey)] = SelectedKey;
    }

    [RelayCommand]
    private void SelectMod1(string? input)
    {
        Mod1 = input ?? "None";
    }

    [RelayCommand]
    private void SelectMod2(string? input)
    {
        Mod2 = input ?? "None";
    }

    [RelayCommand]
    private void SelectKeyCode(string? input)
    {
        KeyCode = input ?? "None";
    }


    private static int? SizePresetToSize(string? sp) => 
        sp == AppResources.SelectSmallSize ? 2 : 
        sp == AppResources.SelectMediumSize ? 3 : 
        sp == AppResources.SelectLargeSize ? 4 : 
        null;
        

    private void Load(KeyboardPreset preset)
    {
        foreach (var item in preset.Keys!)
        {
            var kbi = new KeyPresetVM(item);
            Keys.Add(kbi);
        }
    }

    private void SetKeyModeBase(KeyboardRequestType requestType)
    {
        RequestType = requestType;
        switch (requestType)
        {
            case KeyboardRequestType.SingleKey:
                IsMultiKey2 = false;
                IsMultiKey3 = false;
                IsSingleKey = true;
                break;
            case KeyboardRequestType.MultikeyOneModifier:
                IsMultiKey2 = true;
                IsMultiKey3 = false;
                IsSingleKey = false;
                break;
            case KeyboardRequestType.MultikeyTwoModifiers:
                IsMultiKey2 = false;
                IsMultiKey3 = true;
                IsSingleKey = false;
                break;
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedKey))
        {
            AllowModify = SelectedKey is not null;
            Mod1 = SelectedKey?.Preset.Mod1.ToString();
            Mod2 = SelectedKey?.Preset.Mod2.ToString();
            KeyCode = SelectedKey?.Preset.KeyCode.ToString();
            SetKeyModeBase(SelectedKey?.Preset.RequestType ?? KeyboardRequestType.SingleKey);
            SelectedKeyName = SelectedKey?.Preset.RequestType switch
            {
                KeyboardRequestType.SingleKey => $"{KeyCode}",
                KeyboardRequestType.MultikeyOneModifier => $"{Mod1} + {KeyCode}",
                KeyboardRequestType.MultikeyTwoModifiers => $"{Mod1} + {Mod2} + {KeyCode}",
                _ => string.Empty,
            };
        }
        else if (e.PropertyName == nameof(Mod1))
        {
            if (SelectedKey is not null)
                SelectedKey!.Preset.Mod1 = TotalKeysCollection.ToKeyCode(Mod1);
            SelectedKeyName = SelectedKey?.Preset.RequestType switch
            {
                KeyboardRequestType.SingleKey => $"{KeyCode}",
                KeyboardRequestType.MultikeyOneModifier => $"{Mod1} + {KeyCode}",
                KeyboardRequestType.MultikeyTwoModifiers => $"{Mod1} + {Mod2} + {KeyCode}",
                _ => string.Empty,
            };
        }
        else if (e.PropertyName == nameof(Mod2))
        {
            if (SelectedKey is not null)
                SelectedKey!.Preset.Mod2 = TotalKeysCollection.ToKeyCode(Mod2);
            SelectedKeyName = SelectedKey?.Preset.RequestType switch
            {
                KeyboardRequestType.SingleKey => $"{KeyCode}",
                KeyboardRequestType.MultikeyOneModifier => $"{Mod1} + {KeyCode}",
                KeyboardRequestType.MultikeyTwoModifiers => $"{Mod1} + {Mod2} + {KeyCode}",
                _ => string.Empty,
            };
        }
        else if (e.PropertyName == nameof(KeyCode))
        {
            if (SelectedKey is not null)
                SelectedKey!.Preset.KeyCode = TotalKeysCollection.ToKeyCode(KeyCode);
            SelectedKeyName = SelectedKey?.Preset.RequestType switch
            {
                KeyboardRequestType.SingleKey => $"{KeyCode}",
                KeyboardRequestType.MultikeyOneModifier => $"{Mod1} + {KeyCode}",
                KeyboardRequestType.MultikeyTwoModifiers => $"{Mod1} + {Mod2} + {KeyCode}",
                _ => string.Empty,
            };
        }
        else if (e.PropertyName == nameof(RequestType))
        {
            if (SelectedKey is not null)
                SelectedKey!.Preset.RequestType = RequestType;
            SelectedKeyName = SelectedKey?.Preset.RequestType switch
            {
                KeyboardRequestType.SingleKey => $"{KeyCode}",
                KeyboardRequestType.MultikeyOneModifier => $"{Mod1} + {KeyCode}",
                KeyboardRequestType.MultikeyTwoModifiers => $"{Mod1} + {Mod2} + {KeyCode}",
                _ => string.Empty,
            };
        }
        base.OnPropertyChanged(e);
    }
}
