using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class PlayOutcome
    {
        public readonly Move winningMove;
        public readonly Move losingMove;
        public readonly string description;

        public PlayOutcome(Move winningMove, Move losingMove, string description)
        {
            this.winningMove = winningMove;
            this.losingMove = losingMove;
            this.description = description;
        }
    }

    class PlayCommand : BaseCommand
    {
        Move[] moves;
        PlayOutcome[] outcomes = new PlayOutcome[]
        {
            new PlayOutcome(Move.Scissors, Move.Paper, "{0}'s Scissors cuts {1}'s Paper."),
            new PlayOutcome(Move.Paper, Move.Rock, "{0}'s Paper covers {1}'s Rock."),
            new PlayOutcome(Move.Rock, Move.Lizard, "{0}'s Rock crushes {1}'s Lizard."),
            new PlayOutcome(Move.Lizard, Move.Spock, "{0}'s Lizard poisons {1}'s Spock."),
            new PlayOutcome(Move.Spock, Move.Scissors, "{0}'s Spock smashes {1}'s Scissors."),
            new PlayOutcome(Move.Scissors, Move.Lizard, "{0}'s Scissors decapitates {1}'s Lizard."),
            new PlayOutcome(Move.Lizard, Move.Paper, "{0}'s Lizard eats {1}'s Paper."),
            new PlayOutcome(Move.Paper, Move.Spock, "{0}'s Paper disproves {1}'s Spock."),
            new PlayOutcome(Move.Spock, Move.Rock, "{0}'s Spock vaporizes {1}'s Rock."),
            new PlayOutcome(Move.Rock, Move.Scissors, "{0}'s Rock crushes {1}'s Scissors.")
        };

        public PlayCommand() : base("play")
        {
            moves = (Move[])Enum.GetValues(typeof(Move));
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (stringParams.Count == 0)
            {
                return;
            }

            foreach (var challenge in gameData.Challenges)
            {
                if (challenge.ChallengerUser.Id == message.Author.Id)
                {
                    Move? move = GetMove(stringParams[0]);
                    if (move.HasValue)
                    {
                        challenge.ChallengerMove = move;
                        if (challenge.OpponentMove.HasValue)
                        {
                            End(challenge);
                        }
                        else
                        {
                            UserExtensions.SendMessageAsync(challenge.ChallengerUser, $"Waiting for {challenge.OpponentUser.Username}'s reply.");
                        }
                    }
                }
                else if (challenge.OpponentUser.Id == message.Author.Id)
                {
                    Move? move = GetMove(stringParams[0]);
                    if (move.HasValue)
                    {
                        challenge.OpponentMove = move;
                        if (challenge.ChallengerMove.HasValue)
                        {
                            End(challenge);
                        }
                        else
                        {
                            UserExtensions.SendMessageAsync(challenge.ChallengerUser, $"{challenge.OpponentUser.Username} has accepted your challenge of Rock, Paper, Scissor, Lizard, Spock. Please reply with either *decline* or *play x* where x is your move.");
                        }
                    }
                }
            }
        }

        void End(ChallengeData challenge)
        {
            gameData.Challenges.Remove(challenge);
            if (challenge.ChallengerMove.Value == challenge.OpponentMove.Value)
            {
                challenge.Channel.SendMessageAsync(Util.FormatEvent($"{challenge.ChallengerUser.Username}'s {challenge.ChallengerMove.Value} ties with {challenge.OpponentUser.Username}'s {challenge.OpponentMove.Value}."));
                return;
            }

            SocketUser p1 = challenge.ChallengerUser;
            Move move1 = challenge.ChallengerMove.Value;
            SocketUser p2 = challenge.OpponentUser;
            Move move2 = challenge.OpponentMove.Value;

            PlayOutcome finalOutcome = null;

            foreach (var outcome in outcomes)
            {
                if (outcome.winningMove == move1 && outcome.losingMove == move2)
                {
                    finalOutcome = outcome;
                    break;
                }

                if (outcome.winningMove == move2 && outcome.losingMove == move1)
                {
                    SocketUser pTemp = p1;
                    p1 = p2;
                    p2 = pTemp;

                    Move moveTemp = move1;
                    move1 = move2;
                    move2 = moveTemp;
                    finalOutcome = outcome;
                    break;
                }
            }

            string formattedOutcome = String.Format(finalOutcome.description, p1.Username, p2.Username);
            formattedOutcome += $"\n{p1.Username} defeats {p2.Username}!";
            challenge.Channel.SendMessageAsync(Util.FormatEvent(formattedOutcome));
        }

        Move? GetMove(string moveName)
        {
            moveName = moveName.ToLowerInvariant();
            foreach (var move in moves)
            {
                if (moveName == move.ToString().ToLowerInvariant())
                {
                    return move;
                }
            }
            return null;
        }
    }
}
