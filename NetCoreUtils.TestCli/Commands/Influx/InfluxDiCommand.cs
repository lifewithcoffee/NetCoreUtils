using CoreCmd.Attributes;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using NetCoreUtils.Database.InfluxDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli.Commands.Influx
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

        public async Task Write()
        {
            var random = new Random();
            //await _writer.WriteAsync(new Temperature { Location = "UNSW", Value = 94D, RLTestColumn = 123D, Time = DateTime.UtcNow });
            await _writer.WriteAsync(new Temperature { Location = "UNSW", Value = random.Next(0,100), RLTestColumn = random.Next(0,100), Time = DateTime.UtcNow });
        }

        public async Task WriteList()
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

        public async Task WriteArray(int number)
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
            var point = PointData.Measurement("my-point")
                .Tag("tag1", "PointTag1")
                .Tag("tag2", "PointTag2")
                .Field("value", value)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            await _writer.WriteAsync(point);
            Console.WriteLine($"WritePoint():  {value}");
        }

        public async Task WritePointList()
        {
            List<PointData> list = new List<PointData>();

            for(int i = 0; i < 100; i++)
            {
                var value = new Random().Next(0, 100);

                var point = PointData.Measurement("my-point")
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

                var point = PointData.Measurement("my-point")
                    .Tag("tag1", "PointTag1")
                    .Tag("tag2", "PointTag2")
                    .Field("value", value)
                    .Timestamp(DateTime.UtcNow.AddSeconds(-i), WritePrecision.Ns);

                array[i] = point;
                Console.WriteLine($"WritePointList():  {value}");
            }

            await _writer.WriteAsync(array);
        }

        public async Task Read()
        {
            var temperature = await _reader.QueryAsync<Temperature>(new QueryRange(-7, RangeUnit.day));
            temperature.ForEach(t => { 
                Console.WriteLine(t.ToString());
                Console.WriteLine(t.ToJson());
            });
        }
    }
}
