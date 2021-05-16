using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TestApp.Cli.FifoDemo
{
    class ThreadChannelService
    {
        public async Task ChannelRun(
            int readDelayMs, 
            int writeDelayMs,
            int numberOfReaders,
            int totalMessageNumber,
            int maxCapacity)
        {
            if (readDelayMs < 25)
                readDelayMs = 25;

            if (numberOfReaders < 1)
                numberOfReaders = 1;

            //use a bounded channel is useful if you have a slow consumer
            //unbounded may lead to OutOfMemoryException
            var channel = Channel.CreateBounded<string>(maxCapacity);
            var reader = channel.Reader;
            var writer = channel.Writer;

            Console.WriteLine("=== Initializing reader ...");
            var tasks = new List<Task>();
            for (int i = 0; i < numberOfReaders; i++)
            {
                tasks.Add(Task.Run(() => Read(reader, Guid.NewGuid().ToString().Substring(0,4), readDelayMs)));
                await Task.Delay(10);
            }

            Console.WriteLine("=== Initializing writer ...");

            //Write message to the channel, but since Read has Delay
            //we will get back pressure applied to the writer, which causes it to block
            //when writing. Unbounded channels do not block ever
            for (int i = 0; i < totalMessageNumber; i++)
            {
                Console.WriteLine($"Writing {i} at {DateTime.Now.ToLongTimeString()}");
                await writer.WriteAsync($"SomeText message '{i}");
                await Task.Delay(writeDelayMs);
            }

            //Tell readers writing has finished, they can quit the WaitToReadAsync() loop
            Console.WriteLine("Write completed");
            writer.Complete();

            await Task.WhenAll(tasks);
        }

        public async Task Read(ChannelReader<string> theReader, string readerName, int delayMs)
        {

            Console.WriteLine($"Reader {readerName} started, delayMs = {delayMs}");

            //while when channel is not complete 
            while (await theReader.WaitToReadAsync())
            {
                await foreach (var msg in theReader.ReadAllAsync())
                {
                    Console.WriteLine($"Reader {readerName} read '{msg}' at {DateTime.Now.ToLongTimeString()}");
                    await Task.Delay(delayMs); //simulate some work
                }

                // or implement this way:
                //while (theReader.TryRead(out var theMessage))
                //{
                //    Console.WriteLine($"Reader {readerNumber} read '{theMessage}' at {DateTime.Now.ToLongTimeString()}");
                //    await Task.Delay(delayMs); //simulate some work
                //}
            }

            Console.WriteLine($"Reader {readerName} finished");
        }
    }
}
