using System.Text.Json;
using System.IO;
using System;

namespace TestApp.Cli.Robocopy
{
    public class JsonConfigOperator<T> where T: new()
    {
        public static void Save(string fullPath, T config)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText(fullPath, JsonSerializer.Serialize(config, options));
        }

        public static T LoadCreate(string fullPath)
        {
            if (!File.Exists(fullPath))
                File.Create(fullPath).Close();

            var fileContent = File.ReadAllText(fullPath);
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                return new T();
            }
            else
            {
                return JsonSerializer.Deserialize<T>(fileContent);
            }
        }
    }
}
