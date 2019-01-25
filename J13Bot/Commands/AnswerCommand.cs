using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class AnswerCommand : BaseCommand
    {
        public AnswerCommand() : base("answer")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (gameData.CurrentQuizAnswer.Length > 0 && stringParams.Count > 0)
            {
                string combinedAnswer = string.Join(" ", stringParams);
                if (combinedAnswer.ToLowerInvariant() == gameData.CurrentQuizAnswer.ToLowerInvariant())
                {
                    message.Channel.SendMessageAsync(Util.FormatEvent($"{message.Author} has answered correctly with {gameData.CurrentQuizAnswer}."));
                    gameData.CurrentQuizAnswer = "";
                }
            }
        }
    }
}
