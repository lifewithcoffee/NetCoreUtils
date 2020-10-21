using InfluxDB.Client;
using InfluxDB.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.InfluxDb
{
    public interface IInfluxReader : IInfluxAccess
    {
        Task<List<TMeasurement>> QueryAsync<TMeasurement>(QueryRange range);
    }

    public class InfluxReader : InfluxAccess, IInfluxReader
    {
        QueryApi _queryApi;

        public InfluxReader(IInfluxDbConnection connection)
        {
            _queryApi = connection.Client.GetQueryApi();
        }

        private string GetQueryString<TMeasurement>(QueryRange range)
        {
            var measurementAttribute = (Measurement)typeof(TMeasurement).GetCustomAttribute(typeof(Measurement));
            string measurementName = measurementAttribute.Name;

            string query = $"from(bucket:\"{_bucket}\") |> range(start: {range.ToFluxString()})" +
                           $"|> filter(fn: (r) => r[\"_measurement\"] == \"{measurementName}\")" +
                           $"|> pivot(rowKey:[\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")";

            return query;
        }

        public async Task<List<TMeasurement>> QueryAsync<TMeasurement>(QueryRange range)
        {
            try
            {
                var query = this.GetQueryString<TMeasurement>(range);
                return await _queryApi.QueryAsync<TMeasurement>(query, _org);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
