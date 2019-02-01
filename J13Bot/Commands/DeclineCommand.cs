using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class DeclineCommand : BaseCommand
    {
        public DeclineCommand() : base("decline")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            for (int i = gameData.Challenges.Count - 1; i >= 0 ; i--)
            {
                if (gameData.Challenges[i].ChallengerUser.Id == message.Author.Id || gameData.Challenges[i].OpponentUser.Id == message.Author.Id)
                {
                    gameData.Challenges[i].Channel.SendMessageAsync($"{message.Author.Username} declines the challenge.");
                    gameData.Challenges.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
