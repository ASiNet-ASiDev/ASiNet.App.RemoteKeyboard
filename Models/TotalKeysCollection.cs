using ASiNet.RacAPI.Packages;

namespace ASiNet.App.RemoteKeyboard.Models;
public static class TotalKeysCollection
{
    public static readonly string[] KeyNames = Enum.GetNames<VirtualKeyCode>();

    public static IEnumerable<string> EnumerateKeys(string? filter = null) =>
        filter is null ? KeyNames :
        KeyNames.Where(x => x.Length >= filter.Length && filter.ToLower() == x[..filter.Length].ToLower());

    public static VirtualKeyCode ToKeyCode(string? input) =>
        Enum.Parse<VirtualKeyCode>(input ?? "None");
}