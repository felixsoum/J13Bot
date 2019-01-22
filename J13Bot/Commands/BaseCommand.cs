using Discord.WebSocket;
using J13Bot.Game;
using System.Collections.Generic;

namespace J13Bot
{
    abstract class BaseCommand
    {
        public string Name { get; private set; }
        protected GameData gameData;

        public BaseCommand(string name)
        {
            Name = name;
        }

        public void Init(GameData gameData)
        {
            this.gameData = gameData;
        }

        protected bool IsBotMentioned(SocketUserMessage message)
        {
            foreach (var mentionedUser in message.MentionedUsers)
            {
                if (mentionedUser.Id == Program.MyId)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool IsAuthorOwner(SocketUserMessage message)
        {
            return message.Author.Id == Program.OwnerId;
        }

        protected Player GetAuthorPlayer(SocketUserMessage message)
        {
            if (gameData.IdToPlayer.ContainsKey(message.Author.Id))
            {
                return gameData.IdToPlayer[message.Author.Id];
            }
            else
            {
                return null;
            }
        }

        public abstract void OnCommand(List<string> stringParams, SocketUserMessage message);
    }
}
