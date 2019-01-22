using System.Collections.Generic;
using Discord.WebSocket;
using J13Bot.Game.Items;

namespace J13Bot.Commands
{
    class EatCommand : BaseCommand
    {
        public EatCommand() : base("eat")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (stringParams.Count > 0)
            {
                Player player = GetAuthorPlayer(message);
                if (player != null)
                {
                    BaseItem itemToEat = player.GetItemFromName(stringParams[0]);
                    if (itemToEat != null)
                    {
                        player.Items.Remove(itemToEat);
                        string reply = itemToEat.Eat(message.Author.Username, player);
                        message.Channel.SendMessageAsync(Util.FormatEvent(reply));
                    }
                }
            }
        }
    }
}
