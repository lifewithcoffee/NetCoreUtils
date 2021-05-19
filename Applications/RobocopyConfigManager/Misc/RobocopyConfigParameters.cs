using NetCoreUtils.System;

namespace RobocopyConfigManager.Misc
{
    public static class RobocopyConfigParameters
    {
        public static string fullConfigFilePath { get; }  = @$"{SystemUtil.GetUserDir()}/{RobocopyConfigParameters.ConfigFileName}";
        public static string ConfigFileName { get; } = ".robocopy-backup.config.json";
    }
}
