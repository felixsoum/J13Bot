using System;

namespace J13Bot
{
    class Player
    {
        public int Hp { get; set; } = 100;
        public int LastActionTime { get; set; }

        public int GetActThreshold()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            int secondsPassed = secondsSinceEpoch - LastActionTime;
            int secondsThreshold = 3600 - secondsPassed;
            LastActionTime = secondsSinceEpoch;

            return secondsThreshold;
        }

        public void Damage(int value)
        {
            Hp -= value;
        }

        public void Heal(int value)
        {
            Hp += value;
        }
    }
}
