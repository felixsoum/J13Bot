using System.Collections.Generic;
using System.Threading.Tasks;

namespace J13Bot.Game
{
    class GameData
    {
        public Dictionary<ulong, Player> IdToPlayer { get; internal set; } = new Dictionary<ulong, Player>();
        public TaskCompletionSource<bool> IsGameDone { get; set; } = new TaskCompletionSource<bool>();
        public string CurrentQuizAnswer { get; internal set; } = "";

        public SaveData CreateSaveData()
        {
            var saveData = new SaveData
            {
                playerData = new Player[IdToPlayer.Count]
            };

            int i = 0;
            foreach (var player in IdToPlayer.Values)
            {
                saveData.playerData[i++] = player;
            }

            return saveData;
        }

        public void LoadSaveData(SaveData saveData)
        {
            foreach (var player in saveData.playerData)
            {
                if (IdToPlayer.ContainsKey(player.Id))
                {
                    IdToPlayer[player.Id] = player;
                }
                else
                {
                    IdToPlayer.Add(player.Id, player);
                }
            }
        }
    }
}
