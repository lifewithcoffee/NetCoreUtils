using InfluxDB.Client.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NetCoreUtils.Database.InfluxDb
{
    public class MeasurementBase
    {
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }

        /// <summary>
        /// Only properties with [Column] attributes will be added to the dictionary
        /// </summary>
        protected SortedDictionary<string, object> GetDict()
        {
            var dict = new SortedDictionary<string, object>();

            // add "_measurement" field
            dict.Add("_measurement", this.GetType().GetCustomAttribute<Measurement>().Name);

            // add other property fields with the key names exactly the same with those in the InfluxDB 2.x
            var properties = this.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<Column>();
                if (columnAttr == null) continue;

                string key = columnAttr.Name;   // for properties explicitly specified column names, i.e. applied [Column("some_name")]
                object value = prop.GetValue(this);

                if (key == null)
                {
                    if (columnAttr.IsTimestamp) // for timestamp property
                    {
                        key = "_time";
                        value = ((DateTime)prop.GetValue(this)).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else // for properties without explicit names, i.e. only applied [Column] 
                    {
                        key = prop.Name;
                    }
                }

                dict.Add(key, value);
            }

            return dict;
        }

        public string ToJson()
        {
            var dict = this.GetDict();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Serialize(dict, options);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var dict = this.GetDict();
            foreach (var item in dict)
                sb.Append($"{item.Key} = {item.Value}; ");

            return sb.ToString();
        }

    }
}
