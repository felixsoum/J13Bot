using J13Bot.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace J13Bot
{
    [Serializable]
    class Player : ISerializable
    {
        public ulong Id { get; set; }
        public int Hp { get; set; }

        public int Level { get; set; } = 1;
        public int Gold { get; set; } = 10;

        public int Aggro { get; set; }

        public string Username { get; set; }
        public const int MaxHp = 100;
        public int LastActionTime { get; set; }
#pragma warning disable CA2235 // Mark all non-serializable fields
        public List<BaseItem> Items { get; set; } = new List<BaseItem>();
#pragma warning restore CA2235 // Mark all non-serializable fields

        public Player()
        {
            Hp = MaxHp;
        }

        public Player(SerializationInfo info, StreamingContext context)
        {
            Id = (ulong)info.GetValue(nameof(Id), typeof(ulong));
            Hp = (int)info.GetValue(nameof(Hp), typeof(int));
            Level = (int)info.GetValue(nameof(Level), typeof(int));
            Gold = (int)info.GetValue(nameof(Gold), typeof(int));
            
            LastActionTime = (int)info.GetValue(nameof(LastActionTime), typeof(int));
            string[] itemNames = (string[])info.GetValue(nameof(Items), typeof(string[]));
            Items = itemNames.Select(itemName => ItemDatabase.GetItemByName(itemName)).ToList();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Id), Id, typeof(ulong));
            info.AddValue(nameof(Hp), Hp, typeof(int));
            info.AddValue(nameof(Level), Level, typeof(int));
            info.AddValue(nameof(Gold), Gold, typeof(int));

            info.AddValue(nameof(LastActionTime), LastActionTime, typeof(int));
            string[] itemNames = Items.Select(item => item.Name).ToArray();
            info.AddValue(nameof(Items), itemNames, typeof(string[]));
        }

        public int GetActThreshold()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            int secondsPassed = secondsSinceEpoch - LastActionTime;
            int secondsThreshold = 3600 - secondsPassed;

            return secondsThreshold;
        }

        public void CommitAction()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            LastActionTime = secondsSinceEpoch;
        }

        public string Damage(int value)
        {
            int preHp = Hp;
            Hp -= value;
            if (Hp < 0)
            {
                Hp = 0;
            }

            return Util.FormatEvent($"{Username} loses HP {preHp} => {Hp}");
        }

        public void Heal(int value)
        {
            Hp += value;
        }

        public string GetItemsToString()
        {
            string[] itemStrings = new string[Items.Count];
            for (int i = 0; i < Items.Count; i++)
            {
                itemStrings[i] = Items[i].ToString();
            }
            return String.Join(", ", itemStrings);
        }

        public BaseItem GetItemFromName(string itemName)
        {
            itemName = itemName.ToLowerInvariant();
            return Items.Find(item => itemName == item.Name.ToLowerInvariant());
        }
    }
}
