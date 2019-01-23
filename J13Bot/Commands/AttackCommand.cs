using System;
using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class AttackCommand : BaseCommand
    {
        Random random = new Random();

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
                    string reply = targetPlayer.Damage(10);
                    attacker.CommitAction();
                    message.Channel.SendMessageAsync(reply);
                }
                else
                {
                    message.Channel.SendMessageAsync($"{message.Author} has to wait {secondsThreshold} seconds before acting again.");
                }
            }
            else if (gameData.ActiveMonster != null)
            {
                var attacker = gameData.IdToPlayer[message.Author.Id];
                int secondsThreshold = attacker.GetActThreshold();
                if (secondsThreshold <= 0)
                {
                    int damage = attacker.Level * 5 + random.Next(51);
                    string reply = gameData.ActiveMonster.Damage(damage);
                    attacker.CommitAction();

                    if (gameData.ActiveMonster.Hp <= 0)
                    {
                        attacker.Gold += gameData.ActiveMonster.Gold;
                        reply += Util.FormatEvent($"{attacker.Username} receives {gameData.ActiveMonster.Gold}G for killing {gameData.ActiveMonster.Name}.");
                        gameData.ActiveMonster = null;
                    }

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
