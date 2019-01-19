using System;

namespace J13Bot.Game.Items
{
    class ItemDatabase
    {
        readonly Random random = new Random();
        readonly BaseItem[] items = new BaseItem[]
        {
            new LootboxItem(),
            new StubItem("Dagger", ":dagger:"),
            new StubItem("Poop", ":poop:"),
            new StubItem("Money", ":moneybag:"),
            new StubItem("Boots", ":boot:"),
            new StubItem("Syringe", ":syringe:"),
            new StubItem("Candy", ":candy:"),
            new StubItem("Eggplant", ":eggplant:"),
            new StubItem("Shield", ":shield:"),
            new StubItem("Key", ":key:"),
            new StubItem("Pill", ":pill:"),
        };

        public BaseItem GetLootbox()
        {
            return items[0];
        }

        public BaseItem GetRandomItem()
        {
            return items[random.Next(items.Length)];
        }
    }
}
