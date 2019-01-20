using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class InspectCommand : BaseCommand
    {
        public InspectCommand() : base("inspect")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (message.MentionedUsers.Count > 0)
            {
                SocketUser target = null;
                foreach (var mentionedUser in message.MentionedUsers)
                {
                    if (!mentionedUser.IsBot)
                    {
                        target = mentionedUser;
                        break;
                    }
                }

                if (target != null && gameData.IdToPlayer.ContainsKey(target.Id))
                {
                    Player player = gameData.IdToPlayer[target.Id];
                    string playerItems = player.GetItemsToString();
                    string reply = $"{target.Username}'s stats: HP {player.Hp}";
                    reply += "\nItems: " + playerItems;
                    message.Channel.SendMessageAsync(reply);
                }
            }
        }
    }
}
