using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.InfluxDb
{
    public interface IInfluxWriter : IInfluxAccess
    {
        Task WriteAsync<TMeasurement>(TMeasurement measurement);
    }


    public class InfluxWriter : InfluxAccess, IInfluxWriter
    {
        WriteApiAsync _writeApiAsync;


        public InfluxWriter(IInfluxDbConnection connection)
        {
            _writeApiAsync = connection.Client.GetWriteApiAsync();
        }

        public async Task WriteAsync<TMeasurement>(TMeasurement measurement)
        {
            await _writeApiAsync.WriteMeasurementAsync(_bucket, _org, WritePrecision.Ns, measurement);
        }
    }
}
