using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace Sona.Models
{
    public static class DataService
    {
        private static readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sona");
        private static readonly string SongFilePath = Path.Combine(_folderPath, "songs.json");
        private static readonly string SettingFilePath = Path.Combine(_folderPath, "settings.json");
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
        };

        public static void SaveSongs(ObservableCollection<Song> songs)
        {
            Directory.CreateDirectory(_folderPath);
            string jsonSongs = JsonSerializer.Serialize(songs, Options);
            File.WriteAllText(SongFilePath, jsonSongs);
        }

        //もしsong.jsonファイルが存在しない場合、最初にデフォルトの曲を追加させるためのnull許容
        public static List<Song>? LoadSongs()
        {
            if (!File.Exists(SongFilePath)) return null;
            string jsonSongs = File.ReadAllText(SongFilePath);
            return JsonSerializer.Deserialize<List<Song>>(jsonSongs, Options) ?? new List<Song>();
        }

        public static void SaveSettings(AppSettings appSettings)
        {
            Directory.CreateDirectory(_folderPath);
            string jsonSetting = JsonSerializer.Serialize(appSettings, Options);
            File.WriteAllText(SettingFilePath, jsonSetting);
        }

        public static void LoadSettings()
        {
            if (!File.Exists(SettingFilePath)) return;
            string jsonSetting = File.ReadAllText(SettingFilePath);
            var loaded = JsonSerializer.Deserialize<AppSettings>(jsonSetting, Options);
            if (loaded != null)
            {
                AppSettings.Default.MasterVolume = loaded.MasterVolume;
                AppSettings.Default.SelectedDeviceIds.Clear();
                foreach (var id in loaded.SelectedDeviceIds)
                {
                    AppSettings.Default.SelectedDeviceIds.Add(id);
                }
            }
        }
    }
}
