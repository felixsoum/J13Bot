using System.Collections.Generic;

namespace J13Bot.Game
{
    class GameData
    {
        public Dictionary<ulong, Player> IdToPlayer { get; internal set; } = new Dictionary<ulong, Player>();
        public bool IsDone { get; set; }
    }
}
