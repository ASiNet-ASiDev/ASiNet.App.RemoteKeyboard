namespace ASiNet.App.RemoteKeyboard.Models;
public class KeyboardPreset
{

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Author { get; set; }

    public KeyPreset[]? Keys { get; set; }
}
