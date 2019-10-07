using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace NetCoreUtils.TestCli.JsonDemo
{
    class JsonService
    {
        public void ReadJson(string jsonString)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonString);

                var root = document.RootElement;

                Console.WriteLine($"checksum: {root.GetProperty("checksum").GetString()}");
                //var children = root.GetProperty("roots/bookmark_bar/children");
                var children = root.GetProperty("roots").GetProperty("bookmark_bar").GetProperty("children");

                foreach (var child in children.EnumerateArray())
                {
                    var name = child.GetProperty("name").GetString();
                    Console.WriteLine($"name: {name}");

                    //if(child.TryGetProperty("children", out var children2))
                    //{
                    //    foreach(var child2 in children2.EnumerateArray())
                    //    {
                    //        Console.WriteLine($"    {child2.GetProperty("name").GetString()}");
                    //    }
                    //}
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void PrintJson(ReadOnlySpan<byte> dataUtf8)
        {
            var json = new Utf8JsonReader(dataUtf8, isFinalBlock: true, state: default);

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.ValueSpan;
                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                        break;
                    case JsonTokenType.StartArray:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.String:
                        Console.WriteLine($"STRING: {json.GetString()}");
                        break;
                    case JsonTokenType.Number:
                        if (!json.TryGetInt32(out int valueInteger))
                        {
                            throw new FormatException();
                        }
                        break;
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        Console.WriteLine($"BOOL: {json.GetBoolean()}");
                        break;
                    case JsonTokenType.Null:
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            dataUtf8 = dataUtf8.Slice((int)json.BytesConsumed);
            JsonReaderState state = json.CurrentState;
        }
    }
}
