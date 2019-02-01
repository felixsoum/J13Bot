using Discord.WebSocket;
using J13Bot.Commands;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace J13Bot.Game
{
    class GameData
    {
        public Dictionary<ulong, Player> IdToPlayer { get; internal set; } = new Dictionary<ulong, Player>();
        public TaskCompletionSource<bool> IsGameDone { get; set; } = new TaskCompletionSource<bool>();
        public string CurrentQuizAnswer { get; internal set; } = "";
        public Monster ActiveMonster { get; set; }
        public List<ChallengeData> Challenges { get; set; } = new List<ChallengeData>();

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

        public void Tick()
        {
            int secondsSinceEpoch = Util.GetTime();

            for (int i = Challenges.Count - 1; i >= 0; i--)
            {
                if (secondsSinceEpoch - Challenges[i].ChallengeTime > 300)
                {
                    Challenges.RemoveAt(i);
                }
            }
        }

        internal void CreateChallenge(SocketUser challengerUser, SocketUser opponentUser, ISocketMessageChannel channel)
        {
            Challenges.Add(new ChallengeData(challengerUser, opponentUser, channel));
        }
    }
}
