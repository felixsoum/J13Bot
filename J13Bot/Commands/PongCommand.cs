using Discord.WebSocket;
using System.Collections.Generic;

namespace J13Bot.Commands
{
    class PongCommand : BaseCommand
    {
        public PongCommand() : base("pong")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (stringParams.Count == 0)
            {
                message.Channel.SendMessageAsync("Ping.");
            }
        }
    }
}
