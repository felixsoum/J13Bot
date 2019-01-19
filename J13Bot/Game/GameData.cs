using J13Bot.Game.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace J13Bot.Game
{
    class GameData
    {
        public Dictionary<ulong, Player> IdToPlayer { get; internal set; } = new Dictionary<ulong, Player>();
        public TaskCompletionSource<bool> IsGameDone { get; set; } = new TaskCompletionSource<bool>();
        public ItemDatabase ItemData { get; set; } = new ItemDatabase();
    }
}
