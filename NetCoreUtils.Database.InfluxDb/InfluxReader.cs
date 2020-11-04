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
        Task<PivotData> QueryAsync(string measurementName, QueryRange range);
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

            return this.GetQueryString(measurementName, range);
        }

        private string GetQueryString(string measurementName, QueryRange range)
        {
            return $"from(bucket:\"{_bucket}\") |> range(start: {range.ToFluxString()})" +
                   $"|> filter(fn: (r) => r[\"_measurement\"] == \"{measurementName}\")" +
                   $"|> pivot(rowKey:[\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")";
        }

        public async Task<PivotData> QueryAsync(string measurementName, QueryRange range)
        {
            try
            {
                PivotData result = new PivotData();

                string flux = this.GetQueryString(measurementName, range);
                var fluxTables = await _queryApi.QueryAsync(flux, _org);
                var table = fluxTables.FirstOrDefault();

                if (table != null)
                {
                    string[] excludedColumns = { "result", "table", "_start", "_stop" };

                    foreach(var column in table.Columns)
                    {
                        if(!excludedColumns.Contains(column.Label))
                            result.ColumnTypeInfo[column.Label] = column.DataType;
                    }

                    foreach(var record in table.Records)
                    {
                        Dictionary<string, string> recordData = new Dictionary<string, string>();

                        foreach (var value in record.Values)
                        {
                            var keyName = value.Key.ToString();
                            if(!excludedColumns.Contains(keyName))
                                recordData[keyName] = value.Value.ToString();
                        }

                        result.Records.Add(recordData);
                    }
                }
                else
                    return null;

                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
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
