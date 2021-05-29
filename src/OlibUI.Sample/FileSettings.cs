using System;
using System.IO;
using Newtonsoft.Json;
using OlibUI.Sample.Structures;

namespace OlibUI.Sample
{
    public static class FileSettings
    {
        public static Settings LoadSettings() => JsonConvert.DeserializeObject<Settings>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json"));

        public static void SaveSettings() => File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json", JsonConvert.SerializeObject(Program.Settings));
    }
}
