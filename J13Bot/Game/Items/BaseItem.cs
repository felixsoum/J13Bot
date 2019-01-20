using System;
using System.Collections.Generic;

namespace J13Bot.Game.Items
{
    abstract class BaseItem
    {
        public string Name { get; set; } = "?";
        public string Code { get; set; } = ":question:";

        public BaseItem(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public override string ToString()
        {
            return $"{Code} {Name}";
        }

        public override bool Equals(object obj)
        {
            if (obj is BaseItem otherItem)
            {
                return Name == otherItem.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1719721206;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            return hashCode;
        }
    }
}
