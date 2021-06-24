using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.Database.XunitTest.TestEnv
{
    public class TestGlobalSettings
    {
        public static string JsonSetting { get; }  = "appsettings.json";
        public static string JsonSettingForUnitTest { get; } = "appsettings.UnitTestOnly.json";
    }
}
