using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.InfluxDb
{
    public interface IInfluxWriter : IInfluxAccess
    {
        Task WriteAsync<TMeasurement>(TMeasurement measurement);
        Task WriteAsync<TMeasurement>(List<TMeasurement> measurements);
        Task WriteAsync<TMeasurement>(TMeasurement[] measurements);
        Task WriteAsync(PointData point);
        Task WriteAsync(List<PointData> points);
        Task WriteAsync(PointData[] points);
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

        public async Task WriteAsync<TMeasurement>(List<TMeasurement> measurements)
        {
            await _writeApiAsync.WriteMeasurementsAsync(_bucket, _org, WritePrecision.Ns, measurements);
        }

        public async Task WriteAsync<TMeasurement>(TMeasurement[] measurements)
        {
            await _writeApiAsync.WriteMeasurementsAsync(_bucket, _org, WritePrecision.Ns, measurements);
        }

        public async Task WriteAsync(PointData point)
        {
            await _writeApiAsync.WritePointAsync(_bucket, _org, point);
        }

        public async Task WriteAsync(List<PointData> points)
        {
            await _writeApiAsync.WritePointsAsync(_bucket, _org, points);
        }

        public async Task WriteAsync(PointData[] points)
        {
            await _writeApiAsync.WritePointsAsync(_bucket, _org, points);
        }
    }
}
