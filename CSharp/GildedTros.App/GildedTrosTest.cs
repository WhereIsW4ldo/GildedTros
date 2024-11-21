using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace GildedTros.App
{
    public class GildedTrosTest
    {
        private ICollection<ItemType> _itemTypes;

        public GildedTrosTest()
        {
            var configFilePath = "configuration.json";
            var configFileData = File.ReadAllText(configFilePath);
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };

            _itemTypes = JsonSerializer.Deserialize<List<ItemType>>(configFileData, options) ?? [];
        }
        
        [Fact]
        public void UpdateQuality_GivenRandomItem_ReturnsQualityAndSellInDegraded1()
        {
            // Arrange
            var item = new Item { Name = "SomethingRandom", SellIn = 10, Quality = 10 };
            
            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);
            
            // Assert
            Assert.Equal("SomethingRandom", changedItem.Name);
            Assert.Equal(9, changedItem.SellIn);
            Assert.Equal(9, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenQuality0_ReturnsNonNegativeQuality()
        {
            // Arrange
            var item = new Item { Name = "SomethingRandom", SellIn = 10, Quality = 0 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("SomethingRandom", changedItem.Name);
            Assert.Equal(9, changedItem.SellIn);
            Assert.Equal(0, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenPassedSellIn_ReturnsDoubleDegradation()
        {
            // Arrange
            var item = new Item { Name = "SomethingRandom", SellIn = 0, Quality = 10 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("SomethingRandom", changedItem.Name);
            Assert.Equal(-1, changedItem.SellIn);
            Assert.Equal(8, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenGoodWine_ReturnsImprovedQuality()
        {
            // Arrange
            var item = new Item { Name = "Good Wine", SellIn = 10, Quality = 10 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("Good Wine", changedItem.Name);
            Assert.Equal(9, changedItem.SellIn);
            Assert.Equal(11, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenGoodWineAfterSellIn_ReturnsImprovedDoubleQuality()
        {
            // Arrange
            var item = new Item { Name = "Good Wine", SellIn = 0, Quality = 10 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("Good Wine", changedItem.Name);
            Assert.Equal(-1, changedItem.SellIn);
            Assert.Equal(12, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenGoodWineQuality50_ReturnsQuality50()
        {
            // Arrange
            var item = new Item { Name = "Good Wine", SellIn = 0, Quality = 50 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("Good Wine", changedItem.Name);
            Assert.Equal(-1, changedItem.SellIn);
            Assert.Equal(50, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenLegendary_ReturnsSameQuality()
        {
            // Arrange
            var item = new Item { Name = "B-DAWG Keychain", SellIn = 10, Quality = 80 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("B-DAWG Keychain", changedItem.Name);
            Assert.Equal(9, changedItem.SellIn);
            Assert.Equal(80, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenBackstagePasses_ReturnsIncreaseQuality2SellIn10()
        {
            // Arrange
            var item = new Item { Name = "Backstage passes for Re:factor", SellIn = 10, Quality = 10 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("Backstage passes for Re:factor", changedItem.Name);
            Assert.Equal(9, changedItem.SellIn);
            Assert.Equal(12, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenBackstagePasses_ReturnsIncreaseQuality3SellIn5()
        {
            // Arrange
            var item = new Item { Name = "Backstage passes for Re:factor", SellIn = 5, Quality = 10 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("Backstage passes for Re:factor", changedItem.Name);
            Assert.Equal(4, changedItem.SellIn);
            Assert.Equal(13, changedItem.Quality);
        }

        [Fact]
        public void UpdateQuality_GivenBackstagePasses_ReturnsSetQuality0SellIn0()
        {
            // Arrange
            var item = new Item { Name = "Backstage passes for Re:factor", SellIn = -1, Quality = 10 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal("Backstage passes for Re:factor", changedItem.Name);
            Assert.Equal(-2, changedItem.SellIn);
            Assert.Equal(0, changedItem.Quality);
        }

        [Theory]
        [InlineData("Duplicate Code", 8)]
        [InlineData("Long Methods", 8)]
        [InlineData("Ugly Variable Names", 8)]
        public void UpdateQuality_GivenSmellyItems_ReturnsDoubleDegration(string name, int expectedQuality)
        {
            // Arrange
            var item = new Item { Name = name, SellIn = 10, Quality = 10 };

            // Act
            var changedItem = GildedService.UpdateQuality(item, _itemTypes);

            // Assert
            Assert.Equal(name, changedItem.Name);
            Assert.Equal(9, changedItem.SellIn);
            Assert.Equal(expectedQuality, changedItem.Quality);
        }
    }
}