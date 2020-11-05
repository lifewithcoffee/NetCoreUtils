using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace TestApp.Cli.JsonDemo
{
    class JsonService
    {
        private string PrintIndentSpaces(int level)
        {
            const string indentSpaces = "    ";

            StringBuilder sb = new StringBuilder();
            while(level-- > 0)
                sb.Append(indentSpaces);
            return sb.ToString();
        }

        private int PrintChildNodes(JsonElement element, int currentLevel, int maxLevel)
        {
            int bookmark_count = 0;

            if (currentLevel > maxLevel)
                return 0;

            if(element.TryGetProperty("children", out JsonElement childElement))
            {
                foreach (var child in childElement.EnumerateArray())
                {
                    var name = child.GetProperty("name").GetString();
                    if(child.GetProperty("type").GetString() == "folder")
                    {
                        Console.WriteLine($"{PrintIndentSpaces(currentLevel)}[{name}]");
                    }
                    else
                    {
                        //Console.WriteLine($"{PrintIndentSpaces(currentLevel)}{name}");
                        bookmark_count++;
                    }

                    bookmark_count += PrintChildNodes(child, currentLevel+1, maxLevel);
                }
            }

            return bookmark_count;
        }

        public void PrintVivaldiBookmarkJson(string jsonString, int maxLevel)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonString);

                var root = document.RootElement;

                Console.WriteLine($"checksum: {root.GetProperty("checksum").GetString()}");
                //var children = root.GetProperty("roots/bookmark_bar/children");

                var children = root.GetProperty("roots").GetProperty("bookmark_bar");//.GetProperty("children");
                var count = PrintChildNodes(children, 0, maxLevel);
                Console.WriteLine($"Bookmark number = {count}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void PrintJsonByTokenType(ReadOnlySpan<byte> dataUtf8)
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
