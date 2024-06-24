using System.Text.Json;

namespace ASiNet.App.RemoteKeyboard.Models;
public class Config
{

    public bool Autoconnect { get; set; }

    public int Port { get; set; }

    public string Address { get; set; } = null!;

    public string? Login { get; set; }

    public string? Password { get; set; }


    public static Config Read()
    {
        try
        {
            var directory = Path.Join(FileSystem.AppDataDirectory, "configs");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var filePath = Path.Join(directory, "config.json");

            if (File.Exists(filePath))
            {
                using var file = File.OpenRead(filePath);
                return JsonSerializer.Deserialize<Config>(file) ?? throw new NullReferenceException();
            }
            else
            {
                using var file = File.Create(filePath);
                var cnf = Default;
                JsonSerializer.Serialize(file, cnf);
                return cnf;
            }
        }
        catch
        {
            return Default;
        }
    }

    public static void Write(Config config)
    {
        try
        {
            var directory = Path.Join(FileSystem.AppDataDirectory, "configs");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var filePath = Path.Join(directory, "config.json");
            using var file = File.Create(filePath);
            JsonSerializer.Serialize(file, config);
        }
        catch
        {

        }
    }

    public static Config Default => new() { Port = 45454 };
}
