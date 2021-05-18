namespace RobocopyConfigManager.Misc
{
    public static class RobocopyConfigParameters
    {
        public static string fullConfigFilePath { get; }  = @$"d:\_temp\{RobocopyConfigParameters.ConfigFileName}";
        public static string ConfigFileName { get; } = ".robocopy-backup.config.json";
    }
}
