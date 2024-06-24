using ASiNet.RacAPI.Packages;

namespace ASiNet.App.RemoteKeyboard.Models;

public enum KeyMode
{
    Click,
}

public class KeyPreset 
{
    public string? VisualText { get; set; }

    public int PosX { get; set; }

    public int PosY { get; set; }

    public int SizeX { get; set; } = 2;

    public int SizeY { get; set; } = 2;

    public KeyboardRequestType RequestType { get; set; }

    public VirtualKeyCode Mod1 { get; set; }

    public VirtualKeyCode Mod2 { get; set; }

    public VirtualKeyCode KeyCode { get; set; }

    public KeyMode Mode { get; set; } = KeyMode.Click;
}
