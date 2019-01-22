using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J13Bot.Game.Items
{
    class MoneyBagItem : BaseItem
    {
        public MoneyBagItem() : base("MoneyBag", ":moneybag:")
        {
        }

        public override string Eat(string username, Player eater)
        {
            eater.Gold += 100;
            return $"{username} eats the MoneyBag and gains 100 gold";
        }
    }
}
