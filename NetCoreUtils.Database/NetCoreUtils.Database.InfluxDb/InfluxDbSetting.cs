namespace NetCoreUtils.Database.InfluxDb
{
    public class InfluxDbSetting
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 9999;

        public string Token { get; set; }

        // if Token is not available, username and password will be used
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
