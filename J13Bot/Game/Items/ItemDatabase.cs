using System;
using System.Collections.Generic;

namespace J13Bot.Game.Items
{
    static class ItemDatabase
    {
        static readonly Random random = new Random();
        static readonly BaseItem[] items = new BaseItem[]
        {
            new LootboxItem(),
            new RareCandyItem(),
            new MoneyBagItem(),
            new StubItem("Dagger", ":dagger:"),
            new StubItem("Poop", ":poop:"),
            new StubItem("Boots", ":boot:"),
            new StubItem("HealthPack", ":syringe:"),
            new StubItem("Eggplant", ":eggplant:"),
            new StubItem("Shield", ":shield:"),
            new StubItem("Key", ":key:"),
            new StubItem("Pill", ":pill:"),
            new StubItem("Spoon", ":spoon:"),
            new StubItem("Bow", ":bow_and_arrow:"),
            new StubItem("Gloves", ":boxing_glove:"),
            new StubItem("Guitar", ":guitar:"),
            new StubItem("Flashlight", ":flashlight:"),
            new StubItem("Baguette", ":french_bread:"),
        };
        static readonly Dictionary<string, BaseItem> itemByName = new Dictionary<string, BaseItem>();

        public static BaseItem GetLootbox()
        {
            return items[0];
        }

        public static BaseItem GetRandomItem()
        {
            return items[random.Next(items.Length)];
        }

        public static BaseItem GetItemByName(string name)
        {
            name = name.ToLowerInvariant();

            if (itemByName.Count == 0)
            {
                foreach (var item in items)
                {
                    itemByName.Add(item.Name.ToLowerInvariant(), item);
                }
            }

            if (itemByName.ContainsKey(name))
            {
                return itemByName[name];
            }
            else
            {
                return null;
            }
        }
    }
}
