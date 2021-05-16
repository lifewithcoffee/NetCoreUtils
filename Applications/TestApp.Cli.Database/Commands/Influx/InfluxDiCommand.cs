using CoreCmd.Attributes;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using MongoDB.Bson;
using NetCoreUtils.Database.InfluxDb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestApp.Cli.Database.Commands.Influx
{
    [Alias("idi")]
    public class InfluxDiCommand
    {
        IInfluxWriter _writer;
        IInfluxReader _reader;

        public InfluxDiCommand(IInfluxWriter writer, IInfluxReader reader)
        {
            _writer = writer;
            _reader = reader;
        }

        public async Task WritePoco()
        {
            var random = new Random();
            //await _writer.WriteAsync(new Temperature { Location = "UNSW", Value = 94D, RLTestColumn = 123D, Time = DateTime.UtcNow });
            await _writer.WriteAsync(new Temperature { Location = "UNSW", Value = random.Next(0,100), RLTestColumn = random.Next(0,100), Time = DateTime.UtcNow });
        }

        public async Task WritePocoList()
        {
            List<Temperature> list = new List<Temperature>();
            var random = new Random();

            for(int i = 0; i < 1000; i++)
            {
                list.Add(new Temperature
                         {
                             Location = "WriteList",
                             Value = random.Next(0, 100),
                             RLTestColumn = random.Next(0, 100),
                             Time = DateTime.UtcNow.AddSeconds(0 - random.Next(1, 2000))
                         });
            }

            await _writer.WriteAsync(list);
            Console.WriteLine("WriteList() called");
        }

        public async Task WritePocoArray(int number)
        {
            Temperature[] array = new Temperature[number];
            var random = new Random();

            for(int i = 0; i < number; i++)
            {
                array[i] = new Temperature
                           {
                               Location = "WriteArray",
                               Value = random.Next(0, 100),
                               RLTestColumn = random.Next(0, 100),
                               Time = DateTime.UtcNow.AddSeconds(0 - i)
                           };
                Console.WriteLine($"Value = {array[i].Value}; RLTestColumn = {array[i].RLTestColumn}");
            }
            await _writer.WriteAsync(array);
        }

        public async Task WritePoint()
        {
            var value = new Random().Next(0, 100);

            var point = PointData.Measurement("my-point2")
                .Tag("tag1", "PointTag1")
                .Tag("tag2", "PointTag2")
                .Field("value", value)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            /** Or:
             * var point = PointData.Measurement("my-point2");
             * point = point.Tag("tag1", "PointTag1");  // just doing 'pointData.Tag("tag1", "PointTag1");' doesn't work, this might be a bug 
             * point = point.Tag("tag2", "PointTag2");
             * point = point.Field("value", value);
             * point = point.Timestamp(DateTime.UtcNow, WritePrecision.Ns); 
             **/

            await _writer.WriteAsync(point);
            Console.WriteLine($"WritePoint():  {value}");
        }

        public async Task WritePointModel(string measurementName)
        { 
            var value = new Random().Next(0, 100);

            var point2 = new PointModel<long>();
            point2.Measurement = measurementName;
            point2.Tags["tag3"] = "test-tag31";
            point2.Tags["tag4"] = "test-tag41";
            point2.Fields["value1"] = value;
            point2.Fields["value2"] = value + 2;
            
            // try to (de)serialize to/from json
            Console.WriteLine($"WritePoint(), use System.Text.Json:\n{JsonSerializer.Serialize(point2, new JsonSerializerOptions { WriteIndented = true })}");
            Console.WriteLine($"WritePoint(), use mongodb's ToJson:\n{point2.ToJson()}");

            await _writer.WriteAsync(point2);
        }

        public async Task WritePointModelList(string measurementName, int number)
        {
            var random = new Random();

            List<PointModel<long>> list = new List<PointModel<long>>();

            for(int i = 0; i < number; i++)
            {
                var value = random.Next(0, 100);
                var point =
                    new PointModel<long>
                    {
                        Measurement = measurementName,
                        Tags = { { "tag3", "test-tag31" }, { "tag4", "test-tag41" } },
                        Fields = { { "value1", value }, { "value2", value + 2 } },
                        Timestamp = DateTime.UtcNow.AddSeconds(-i)
                    };

                list.Add(point);
                Console.WriteLine(point.ConverToPointData().ToLineProtocol());
            }

            await _writer.WriteAsync(list);
        }

        public async Task WritePointModelArray(string measurementName, int number)
        {
            var random = new Random();

            PointModel<long>[] array = new PointModel<long>[number];

            for (int i = 0; i < number; i++)
            {
                var value = random.Next(0, 100);
                var point =
                    new PointModel<long>
                    {
                        Measurement = measurementName,
                        Tags = { { "tag3", "test-tag31" }, { "tag4", "test-tag41" } },
                        Fields = { { "value1", value }, { "value2", value + 2 } },
                        Timestamp = DateTime.UtcNow.AddSeconds(-i)
                    };

                array[i] = point;
                Console.WriteLine(point.ConverToPointData().ToLineProtocol());
            }

            await _writer.WriteAsync(array);
        }

        public async Task WriteDeserializedPoint(string measurementName)
        {
            try
            {
                string dateTimeString = JsonSerializer.Serialize(DateTime.UtcNow);
                Console.WriteLine($"Calling WriteDeserializedPoint()...{dateTimeString}");

                string json =
                    "{\"Measurement\": \""
                    + measurementName
                    + "\", \"Tags\": { \"tag3\": \"test-tag31\", \"tag4\": \"test-tag41\" }, \"Fields\": { \"value1\": 28, \"value2\": 30 }, \"Timestamp\": "
                    + dateTimeString
                    + "}";
                //+ "2020-10-29T01:32:31.8118638Z\" }"
                Console.WriteLine($"json string:\n{json}");

                var point3 = JsonSerializer.Deserialize<PointModel<long>>(json);

                await _writer.WriteAsync(point3);

                var point4 = point3.ConverToPointData();

                Debug.Assert(point3.Measurement == measurementName);
                Debug.Assert(point3.Tags["tag3"] == "test-tag31");
                Debug.Assert(point3.Tags["tag4"] == "test-tag41");
                Debug.Assert(point3.Fields["value1"] == 28);
                Debug.Assert(point3.Fields["value2"] == 30);

                Console.WriteLine("All correct");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task WritePointList()
        {
            List<PointData> list = new List<PointData>();

            for(int i = 0; i < 100; i++)
            {
                var value = new Random().Next(0, 100);

                var point = PointData.Measurement("point-list")
                    .Tag("tag1", "PointTag1")
                    .Tag("tag2", "PointTag2")
                    .Field("value", value)
                    .Timestamp(DateTime.UtcNow.AddSeconds(-i), WritePrecision.Ns);

                list.Add(point);
                Console.WriteLine($"WritePointList():  {value}");
            }

            await _writer.WriteAsync(list);
        }

        public async Task WritePointArray()
        {
            PointData[] array = new PointData[100];

            for(int i = 0; i < 100; i++)
            {
                var value = new Random().Next(0, 100);

                var point = PointData.Measurement("point-array")
                    .Tag("tag1", "PointTag1")
                    .Tag("tag2", "PointTag2")
                    .Field("value", value)
                    .Timestamp(DateTime.UtcNow.AddSeconds(-i), WritePrecision.Ns);

                array[i] = point;
                Console.WriteLine($"WritePointList():  {value}");
            }

            await _writer.WriteAsync(array);
        }

        public async Task ReadPoco()
        {
            var temperature = await _reader.QueryAsync<Temperature>(new QueryRange(-7, RangeUnit.day));
            temperature.ForEach(t => { 
                Console.WriteLine(t.ToString());
                Console.WriteLine(t.ToJson());
            });
        }

        public async Task ReadPivotData(string measurementName)
        {
            PivotData data = await _reader.QueryAsync(measurementName, new QueryRange(-7, RangeUnit.day));
            Console.WriteLine(data.ToJson());
            Console.WriteLine("-----------------");
       }
    }

    [Measurement("mmt1")]
    class Mmt : MeasurementBase
    {
        [Column("tag3", IsTag = true)] public string Tag3 { get; set; }
        [Column("tag4", IsTag = true)] public string Tag4 { get; set; }

        [Column("value1")] public long Value1 { get; set; }
        [Column("value2")] public long Value2 { get; set; }
    }
}
