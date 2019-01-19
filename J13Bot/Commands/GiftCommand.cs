using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class GiftCommand : BaseCommand
    {
        public GiftCommand() : base("gift")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (!IsAuthorOwner(message))
            {
                return;
            }
            if (stringParams.Count > 0 && stringParams[0] == "everyone")
            {
                foreach (var player in gameData.IdToPlayer.Values)
                {
                    player.Items.Add(gameData.ItemData.GetLootbox());
                }
                message.Channel.SendMessageAsync(Reply("Everyone"));
            }
            else if (message.MentionedUsers.Count > 0)
            {
                SocketUser target = null;
                foreach (var user in message.MentionedUsers)
                {
                    if (!user.IsBot)
                    {
                        target = user;
                        break;
                    }
                }
                if (target != null && gameData.IdToPlayer.ContainsKey(target.Id))
                {
                    Player targetPlayer = gameData.IdToPlayer[target.Id];
                    targetPlayer?.Items.Add(gameData.ItemData.GetLootbox());
                    message.Channel.SendMessageAsync(Reply(target.Username));
                }
            }
        }

        string Reply(string user)
        {
            return $"{user} has received a lootbox. Please use command: open lootbox";
        }
    }
}
