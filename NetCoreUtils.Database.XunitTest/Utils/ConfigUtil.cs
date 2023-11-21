using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.XunitTest.Utils;

internal class ConfigUtil
{
    public static IConfigurationRoot AppSettings
    {
        get
        {
            /**
             * - If need to add more json files, call .AddJsonFile() multiple times
             * - If need to apply secrets, apply .AddUserSecrets(userSecretsId: "<secret-id>") before .Build()
             */
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(_SettingFileDir)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            return configuration;
        }
    }

    private static string _setting_file_dir = null;
    private static string _SettingFileDir
    {
        get
        {
            if(_setting_file_dir == null)
            {
                string projectDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
                _setting_file_dir = Path.Combine(projectDir, ".");
            }

            return _setting_file_dir;
        }
    }
}
