using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class AttackCommand : BaseCommand
    {
        public AttackCommand() : base("attack")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            SocketUser target = null;
            foreach (var mentionedUser in message.MentionedUsers)
            {
                target = mentionedUser;
            }

            if (target != null)
            {
                if (target.IsBot)
                {
                    message.Channel.SendMessageAsync($"Attacking non-humans is not allowed.");
                    return;
                }

                var attacker = gameData.IdToPlayer[message.Author.Id];
                int secondsThreshold = attacker.GetActThreshold();
                if (secondsThreshold <= 0)
                {
                    var targetPlayer = gameData.IdToPlayer[target.Id];
                    int preHp = targetPlayer.Hp;
                    targetPlayer.Damage(10);
                    string reply = "```excel";
                    reply += "\n" + $"{target.Username} loses HP {preHp} => {targetPlayer.Hp}";
                    reply += "\n```";
                    message.Channel.SendMessageAsync(reply);
                }
                else
                {
                    message.Channel.SendMessageAsync($"{message.Author} has to wait {secondsThreshold} seconds before acting again.");
                }
            }
        }
    }
}
