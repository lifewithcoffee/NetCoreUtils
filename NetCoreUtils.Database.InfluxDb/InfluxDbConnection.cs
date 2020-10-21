using InfluxDB.Client;
using System;

namespace NetCoreUtils.Database.InfluxDb
{
    public interface IInfluxDbConnection
    {
        void UpdateSetting(InfluxDbSetting setting);
        InfluxDBClient Client { get; }
    }

    public class InfluxDbConnection : IInfluxDbConnection, IDisposable
    {
        InfluxDBClient _influxDBClient;

        public InfluxDBClient Client => _influxDBClient;

        public InfluxDbConnection(InfluxDbSetting setting)
        {
            this.UpdateSetting(setting);
        }

        public void UpdateSetting(InfluxDbSetting setting)
        {
            this.Dispose();

            if(string.IsNullOrWhiteSpace(setting.Token))
            {
                _influxDBClient = InfluxDBClientFactory.Create(
                    $"http://{setting.Host}:{setting.Port}"
                    , setting.Username
                    , setting.Password.ToCharArray()
                    );
            }
            else
            {
                _influxDBClient = InfluxDBClientFactory.Create(
                    $"http://{setting.Host}:{setting.Port}"
                    , setting.Token.ToCharArray()
                    );
            }
        }

        public void Dispose()
        {
            if (this.Client != null)
            {
                this.Client.Dispose();
            }
        }
    }
}
