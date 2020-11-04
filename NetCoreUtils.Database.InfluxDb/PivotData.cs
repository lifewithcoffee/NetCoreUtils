using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetCoreUtils.Database.InfluxDb
{
    public class PivotData
    {
        /// <summary>
        /// [label, type]
        /// </summary>
        [JsonPropertyName("column_type_info")]
        public Dictionary<string, string> ColumnTypeInfo { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("records")]
        public List<Dictionary<string, string>> Records { get; set; } = new List<Dictionary<string, string>>();

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Serialize(this, options);
        }
    }
}
