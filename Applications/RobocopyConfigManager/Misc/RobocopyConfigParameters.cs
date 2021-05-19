using NetCoreUtils.OS;

namespace RobocopyConfigManager.Misc
{
    public static class RobocopyConfigParameters
    {
        public static string fullConfigFilePath
        {
            get
            {
                return @$"{SystemUtil.GetUserDir()}/{RobocopyConfigParameters.ConfigFileName}";

            }
        }

        public static string ConfigFileName { get; } = ".robocopy-backup.config.json";
    }
}
