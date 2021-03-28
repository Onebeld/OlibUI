using System;
using System.IO;
using System.Text.Json;
using OlibUI.Sample.Structures;

namespace OlibUI.Sample
{
    public static class FileSettings
    {
        public static Settings LoadSettings() => JsonSerializer.Deserialize<Settings>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json"));

        public static void SaveSettings() => File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json", JsonSerializer.Serialize(Program.Settings));
    }
}
