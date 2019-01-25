using System;
using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class QuizEntry
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public QuizEntry(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }

    class QuizCommand : BaseCommand
    {
        Random random = new Random();
        List<string> gameNames = new List<string>()
        {
            "Overwatch",
            "Minecraft",
            "Monster Hunter",
            "Counter strike",
            "God of War",
            "Red Dead Redemption",
            "Far Cry",
            "Super Smash Bros",
            "Assassin's Creed",
            "Fallout",
            "Tetris",
            "Tomb Raider",
            "Celeste",
            "Fornite",
            "Cuphead",
            "Horizon Zero Dawn",
            "Destiny",
            "FIFA"
        };

        public QuizCommand() : base("quiz")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            int tries = 3;
            while (tries-- > 0)
            {
                string randomName = gameNames[random.Next(gameNames.Count)];
                gameData.CurrentQuizAnswer = randomName;

                string url = ImgurCommand.RequestToImgUrl(randomName + " screenshot");

                if (url.Length == 0)
                {
                    continue;
                }

                string reply = $"What is the name of the following game?\n";
                reply += url;
                message.Channel.SendMessageAsync(reply);
                break;
            }
        }
    }
}
