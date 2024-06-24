using System.Diagnostics;
using ASiNet.Data.Serialization.V2;
using ASiNet.Data.Serialization.V2.Extensions;
using ASiNet.Data.Serialization.V2.IO;
using CommunityToolkit.Maui.Storage;

namespace ASiNet.App.RemoteKeyboard.Models;
public class KeyboardsManager
{

    public KeyboardsManager()
    {
        if(!Directory.Exists(Path.Join(FileSystem.Current.AppDataDirectory, "keyboards")))
            Directory.CreateDirectory(Path.Join(FileSystem.Current.AppDataDirectory, "keyboards"));
        _current = new SerializerBuilder<ushort>()
            .SetIndexer(new TcpLib.SerializerIndexer())
            .RegisterBaseTypes()
            .AllowRecursiveTypeDeconstruction()
            .RegisterType<KeyboardPreset>()
            .Build();
    }

    private ISerializer _current;


    public async Task<bool> ImportPreset(string path) =>
        await Task.Run(() =>
        {
            try
            {
                var fi = new FileInfo(path);
                File.Copy(path, Path.Join(FileSystem.Current.AppDataDirectory, "keyboards", fi.Name));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        });

    public async Task<bool> ExportPreset(KeyboardPreset preset)
    {
        try
        {
            using var file = File.OpenRead(Path.Join(FileSystem.Current.AppDataDirectory, "keyboards", $"{preset.Title}_{preset.Author}.remkeyprt"));

            await FileSaver.Default.SaveAsync($"{preset.Title}_{preset.Author}.remkeyprt", file, default);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return false;
        }
    }

    public IEnumerable<KeyboardPreset> EnumerateKeyboards()
    {
        var presets = Path.Join(FileSystem.Current.AppDataDirectory, "keyboards");
        foreach (var preset in Directory.EnumerateFiles(presets))
        {
            KeyboardPreset? result = null;
            try
            {
                using var fs = File.OpenRead(preset);
                result = _current.Deserialize<KeyboardPreset>((SerializerFileStreamIO)fs);
            }
            catch
            {
                result = null;
            }
            if (result is not null)
                yield return result;
        }
        yield break;
    }

    public async Task<bool> Remove(KeyboardPreset preset) =>
         await Task.Run(() =>
         {
             try
             {
                 var presets = Path.Join(FileSystem.Current.AppDataDirectory, "keyboards");
                 File.Delete(Path.Join(presets, $"{preset.Title}_{preset.Author}.remkeyprt"));
                 return true;
             }
             catch (Exception ex)
             {
                 Debug.WriteLine(ex.ToString());
                 return false;
             }
         });

    public async Task<bool> SaveOrUpdate(KeyboardPreset preset) =>
        await Task.Run(() =>
        {
            try
            {
                var presets = Path.Join(FileSystem.Current.AppDataDirectory, "keyboards");
                using var fs = File.Create(Path.Join(presets, $"{preset.Title}_{preset.Author}.remkeyprt"));
                _current.Serialize(preset, (SerializerFileStreamIO)fs);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        });

    public KeyboardPreset? LoadBackup()
    {
        try
        {
            var preset = Path.Join(FileSystem.Current.AppDataDirectory, "bk", "bk.remkeyprt");

            if(!File.Exists(preset))
                return null;
        
            var fs = File.OpenRead(preset);
            var result = _current.Deserialize<KeyboardPreset>((SerializerFileStreamIO)fs);
            fs.Dispose();

            File.Delete(preset);

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }

    public async Task<bool> CreateBackup(KeyboardPreset preset) =>
        await Task.Run(() =>
        {
            try
            {
                var path = Path.Join(FileSystem.Current.AppDataDirectory, "bk", "bk.remkeyprt");

                if (!Directory.Exists(Path.Join(FileSystem.Current.AppDataDirectory, "bk")))
                    Directory.CreateDirectory(Path.Join(FileSystem.Current.AppDataDirectory, "bk"));

                using var fs = File.Create(path);
                _current.Serialize(preset, (SerializerFileStreamIO)fs);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        });

}
