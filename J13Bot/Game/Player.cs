using J13Bot.Game.Items;
using System;
using System.Collections.Generic;

namespace J13Bot
{
    class Player
    {
        public int Hp { get; set; } = 100;
        public int LastActionTime { get; set; }
        public List<BaseItem> Items { get; set; } = new List<BaseItem>();

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
