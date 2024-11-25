using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GildedTros.App;

public class ItemType
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("defaultQualityChangeOverTime")]
    public int DefaultQualityChangeOverTime { get; set; } = -1;

    [JsonPropertyName("examples")]
    public IEnumerable<string> ItemNames { get; set; } = [];

    [JsonPropertyName("customChange")]
    public IDictionary<string, int> CustomChange { get; set; } = new Dictionary<string, int>();

    [JsonPropertyName("customAfterSellIn")]    
    public CustomAfterSellIn? SetAfterSellIn { get; set; }

    public static ItemType Default => new() { Type = "Default", DefaultQualityChangeOverTime = -1 };

    public int GetQualityChangeOverTime(Item item)
    {
        var normalChange = CustomChange.TryGetValue(item.SellIn.ToString(), out var change) 
            ? change 
            : DefaultQualityChangeOverTime;

        if (SetAfterSellIn is not null && item.SellIn < 0)
        {
            switch (SetAfterSellIn)
            {
                case CustomAfterSellIn.SetToZero:
                    return 0;
                    break;
            }
        }

        return item.SellIn < 0
            ? item.Quality + normalChange * 2
            : item.Quality + normalChange;
    }
    
    
}

public enum CustomAfterSellIn
{
    SetToZero
}