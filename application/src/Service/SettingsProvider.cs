using System;
using System.IO;
using System.Text.Json;

namespace teamslink
{

    class Settings
    {
        public string appId { get; set; }
        public string tenantId { get; set; }
    }

    class SettingsProvider
    {

        private static readonly string[] _settingsFiles = new[] { Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.tmg.json", "/etc/tmg.json" };

        public static Settings GetSettings()
        {

            foreach (var settingsFileLocation in _settingsFiles)
            {
                if (File.Exists(settingsFileLocation))
                {
                    try
                    {
                        var json = File.ReadAllText(settingsFileLocation);
                        var settings = JsonSerializer.Deserialize<Settings>(json);
                        return settings;
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Unknown error occurred while reading " + settingsFileLocation + " will try next one.");
                    }
                }
            }
            throw new Layer8Exception("No configuration settings found. Please make sure a valid configuration is set in either file: " + String.Join(", ", _settingsFiles));
        }
    }
}
