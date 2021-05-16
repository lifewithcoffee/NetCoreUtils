using NetCoreUtils.Database.InfluxDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TestApp.Cli.Database.Commands.Influx
{
    class InfluxJsonCommand
    {
        public void PointModel()
        {
            PointModel<long> model = new PointModel<long>();
            model.Measurement = "TestMeasurement";
            model.Tags["TestTag1"] = "SomeTag1";
            model.Tags["TestTag2"] = "SomeTag2";
            model.Tags["TestTag3"] = "SomeTag3";
            model.Fields["TestValu1"] = 12345;
            model.Fields["TestValu2"] = 54321;

            string json = JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);

            //PointModel<long> model2 = JsonSerializer.Deserialize<PointModel<long>>(json);
        }
    }
}
