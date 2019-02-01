using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    enum Move
    {
        Rock, Paper, Scissors, Lizard, Spock
    }

    class ChallengeData
    {
        public SocketUser ChallengerUser { get; set; }
        public Move? ChallengerMove { get; set; }
        public SocketUser OpponentUser { get; set; }
        public Move? OpponentMove { get; set; }
        public int ChallengeTime { get; set; }
        public ISocketMessageChannel Channel { get; set; }

        public ChallengeData(SocketUser challengerUser, SocketUser opponentUser, ISocketMessageChannel channel)
        {
            ChallengerUser = challengerUser;
            OpponentUser = opponentUser;
            Channel = channel;
            ChallengeTime = Util.GetTime();
        }
    }

    class ChallengeCommand : BaseCommand
    {
        public ChallengeCommand() : base("challenge")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (message.MentionedUsers.Count == 0)
            {
                return;
            }


            SocketUser opponent = null;
            foreach (var user in message.MentionedUsers)
            {
                opponent = user;
                break;
            }

            if (message.Author.Id == opponent.Id)
            {
                return;
            }

            foreach (var challenges in gameData.Challenges)
            {
                if (challenges.ChallengerUser.Id == message.Author.Id || challenges.OpponentUser.Id == message.Author.Id)
                {
                    message.Channel.SendMessageAsync($"{message.Author.Username} you are currently busy in another challenge.");
                    return;
                }
                else if (challenges.ChallengerUser.Id == opponent.Id || challenges.OpponentUser.Id == opponent.Id)
                {
                    message.Channel.SendMessageAsync($"{opponent.Username} is currently busy in another challenge.");
                    return;
                }
            }
            gameData.CreateChallenge(message.Author, opponent, message.Channel);
            UserExtensions.SendMessageAsync(opponent, $"{message.Author.Username} has challenged you to a game of Rock, Paper, Scissors, Lizard, Spock. Please reply with either *decline* or *play x* where x is your move.");
        }
    }
}
