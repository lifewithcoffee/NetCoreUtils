using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace NetCoreUtils.Database.InfluxDb
{
    /// <summary>
    /// Point model for json serialization.
    /// </summary>
    /// <typeparam name="T">The field data type.</typeparam>
    public class PointModel<T>
    {
        public string Measurement { get; set; }
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, T> Fields { get; set; } = new Dictionary<string, T>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public PointData ConverToPointData()
        {
            var pointData = PointData.Measurement(Measurement);

            foreach(var tag in Tags)
            {
                pointData = pointData.Tag(tag.Key, tag.Value);  // just doing "pointData.Tag(tag.Key, tag.Value);" doesn't work, this might be a bug
            }

            foreach(var field in Fields)
            {
                switch (field.Value)
                {
                    case long value:
                        pointData = pointData.Field(field.Key, value); break;
                    case ulong value:
                        pointData = pointData.Field(field.Key, value); break;
                    case double value:
                        pointData = pointData.Field(field.Key, value); break;
                    case float value:
                        pointData = pointData.Field(field.Key, value); break;
                    case decimal value:
                        pointData = pointData.Field(field.Key, value); break;
                    case bool value:
                        pointData = pointData.Field(field.Key, value); break;
                    case int value:
                        pointData = pointData.Field(field.Key, value); break;
                    case uint value:
                        pointData = pointData.Field(field.Key, value); break;
                    case string value:
                        pointData = pointData.Field(field.Key, value); break;
                    default:
                        throw new Exception("Unknown field type");
                }
            }

            pointData = pointData.Timestamp(Timestamp, WritePrecision.Ns);

            return pointData;
        }
    }
}
