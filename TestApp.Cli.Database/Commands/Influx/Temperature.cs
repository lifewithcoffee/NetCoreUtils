using InfluxDB.Client.Core;
using NetCoreUtils.Database.InfluxDb;

namespace NetCoreUtils.TestCli.Commands.Influx
{
    [Measurement("temperature")]
    class Temperature : MeasurementBase
    {
        [Column("location", IsTag = true)] public string Location { get; set; }
        [Column("value")] public double Value { get; set; }
        [Column("rl_test")] public double RLTestColumn { get; set; }
        //[Column] public double Hello { get; set; }
        //public double Hello { get; set; }
    }
}
