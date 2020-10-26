using NetCoreUtils.Database.InfluxDb;
using System;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli.Commands.Influx
{
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
