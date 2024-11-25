using System.Collections.Generic;
using System.Linq;

namespace GildedTros.App;

public static class GildedService
{
    public static void UpdateQuality(IEnumerable<Item> items, ICollection<ItemType> itemTypes)
    {
        foreach (var item in items)
            UpdateQuality(item, itemTypes);
    }
    
    public static Item UpdateQuality(Item item, ICollection<ItemType> itemTypes)
    {
        var originalQuality = item.Quality;
        item.SellIn -= 1;

        var itemType = itemTypes.FirstOrDefault(x => x.ItemNames.Contains(item.Name))
            ?? ItemType.Default;
        
        var newQuality = itemType.GetQualityChangeOverTime(item);

        // No changes over 50, but items already bigger than 50 okay
        return CapQuality(item, newQuality, originalQuality);
    }

    private static Item CapQuality(Item item, int newQuality, int originalQuality)
    {
        switch (newQuality)
        {
            case > 50 when originalQuality <= 50:
                item.Quality = 50;
                return item;
            case < 0:
                item.Quality = 0;
                return item;
            default:
                item.Quality = newQuality;
                return item;
        }
    }
}