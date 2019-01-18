using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class HelpCommand : BaseCommand
    {
        public HelpCommand() : base("help")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (IsBotMentioned(message))
            {
                message.Channel.SendMessageAsync($"Readme and source at https://github.com/felixsoum/J13Bot");
            }
        }
    }
}
