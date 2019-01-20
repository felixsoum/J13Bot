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
            Id = (ulong)info.GetValue("id", typeof(ulong));
            Hp = (int)info.GetValue("hp", typeof(int));
            LastActionTime = (int)info.GetValue("lastActionTime", typeof(int));
            string[] itemNames = (string[])info.GetValue("items", typeof(string[]));
            Items = itemNames.Select(itemName => ItemDatabase.GetItemByName(itemName)).ToList();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", Id, typeof(ulong));
            info.AddValue("hp", Hp, typeof(int));
            info.AddValue("lastActionTime", LastActionTime, typeof(int));
            string[] itemNames = Items.Select(item => item.Name).ToArray();
            info.AddValue("items", itemNames, typeof(string[]));
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

        public void Damage(int value)
        {
            Hp -= value;
            if (Hp < 0)
            {
                Hp = 0;
            }
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
    }
}
