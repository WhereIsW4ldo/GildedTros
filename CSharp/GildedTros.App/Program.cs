using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic.FileIO;
using FileSystem = System.IO.Abstractions.FileSystem;

namespace GildedTros.App
{
    class Program
    {
        public static void Main()
        {
            IFileSystem fileSystem = new FileSystem();
            
            var configFile = "configuration.json";
            var configFileData = fileSystem.File.ReadAllText(configFile);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            
            var itemTypes = JsonSerializer.Deserialize<List<ItemType>>(configFileData, options)
                ?? [];

            foreach (var itemType in itemTypes)
            {
                Console.WriteLine(JsonSerializer.Serialize(itemType, options));
            }
        }

        private static void TestMain()
        {
            Console.WriteLine("OMGHAI!");

            var items = new List<Item>
            {
                new() { Name = "Ring of Cleansening Code", SellIn = 10, Quality = 20 },
                new() { Name = "Good Wine", SellIn = 2, Quality = 0 },
                new() { Name = "Elixir of the SOLID", SellIn = 5, Quality = 7 },
                new() { Name = "B-DAWG Keychain", SellIn = 0, Quality = 80 },
                new() { Name = "B-DAWG Keychain", SellIn = -1, Quality = 80 },
                new() { Name = "Backstage passes for Re:factor", SellIn = 15, Quality = 20 },
                new() { Name = "Backstage passes for Re:factor", SellIn = 10, Quality = 49 },
                new() { Name = "Backstage passes for HAXX", SellIn = 5, Quality = 49 },
                // these smelly items do not work properly yet
                new() { Name = "Duplicate Code", SellIn = 3, Quality = 6 },
                new() { Name = "Long Methods", SellIn = 3, Quality = 6 },
                new() { Name = "Ugly Variable Names", SellIn = 3, Quality = 6 }
            };

            var app = new GildedTros
            {
                Items = items
            };

            for (var i = 0; i < 31; i++)
            {
                Console.WriteLine("-------- day " + i + " --------");
                Console.WriteLine("name, sellIn, quality");
                foreach (var item in items)
                {
                    Console.WriteLine(item.Name + ", " + item.SellIn + ", " + item.Quality);
                }

                Console.WriteLine("");
                app.UpdateQuality();
            }
        }
    }
}
