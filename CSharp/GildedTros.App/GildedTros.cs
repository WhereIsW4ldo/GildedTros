using System.Collections.Generic;

namespace GildedTros.App
{
    public class GildedTros
    {
        public ICollection<Item> Items { get; set; } = new List<Item>();

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                UpdateItem(item);
            }
        }

        private static void UpdateItem(Item item)
        {
            if (item.Name != "Good Wine" 
                && item.Name != "Backstage passes for Re:factor"
                && item.Name != "Backstage passes for HAXX")
            {
                if (item.Quality > 0)
                {
                    if (item.Name != "B-DAWG Keychain")
                    {
                        item.Quality -= 1;
                    }
                }
            }
            else
            {
                if (item.Quality < 50)
                {
                    item.Quality += 1;

                    if (item.Name == "Backstage passes for Re:factor"
                        || item.Name == "Backstage passes for HAXX")
                    {
                        if (item.SellIn < 11)
                        {
                            if (item.Quality < 50)
                            {
                                item.Quality += 1;
                            }
                        }

                        if (item.SellIn < 6)
                        {
                            if (item.Quality < 50)
                            {
                                item.Quality += 1;
                            }
                        }
                    }
                }
            }

            if (item.Name != "B-DAWG Keychain")
            {
                item.SellIn -= 1;
            }

            if (item.SellIn < 0)
            {
                if (item.Name != "Good Wine")
                {
                    if (item.Name != "Backstage passes for Re:factor"
                        && item.Name != "Backstage passes for HAXX")
                    {
                        if (item.Quality > 0)
                        {
                            if (item.Name != "B-DAWG Keychain")
                            {
                                item.Quality -= 1;
                            }
                        }
                    }
                    else
                    {
                        item.Quality -= item.Quality;
                    }
                }
                else
                {
                    if (item.Quality < 50)
                    {
                        item.Quality += 1;
                    }
                }
            }
        }
    }
}
