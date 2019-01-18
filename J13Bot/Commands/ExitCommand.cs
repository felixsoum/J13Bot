using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class ExitCommand : BaseCommand
    {
        public ExitCommand() : base("exit")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (IsAuthorOwner(message))
            {
                message.Channel.SendMessageAsync($"Initiating maintenance mode.");
            }
        }
    }
}
