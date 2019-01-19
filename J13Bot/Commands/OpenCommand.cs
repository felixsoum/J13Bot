using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using J13Bot.Game.Items;

namespace J13Bot.Commands
{
    class OpenCommand : BaseCommand
    {
        public OpenCommand() : base("open")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (stringParams.Count > 0 && gameData.IdToPlayer.ContainsKey(message.Author.Id))
            {
                if (stringParams[0] == "lootbox")
                {
                    Player player = gameData.IdToPlayer[message.Author.Id];
                    BaseItem lootbox = player.Items.Find(item => item.Name.ToLowerInvariant() == "lootbox");
                    if (lootbox != null)
                    {
                        player.Items.Remove(lootbox);
                        BaseItem newItem = gameData.ItemData.GetRandomItem();
                        player.Items.Add(newItem);
                        string reply = $"{message.Author.Username} opened a lootbox and obtained :gift: => {newItem.ToString()}";
                        message.Channel.SendMessageAsync(reply);
                    }
                    else
                    {
                        message.Channel.SendMessageAsync($"{message.Author.Username}, you do not have a lootbox.");
                    }
                }
            }
        }
    }
}
