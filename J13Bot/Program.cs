using Discord;
using Discord.WebSocket;
using J13Bot.Commands;
using J13Bot.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace J13Bot
{
    class Program
    {
        const ulong GeneralId = 516765447975206915;
        const ulong TestId = 535665547262558238;
        const ulong GuildId = 516765447975206913;
        public const ulong MyId = 535633023140495380;
        public const ulong OwnerId = 139024035894788096;
        const string thinkingEmote = "<:thinking_2B:535689034307993631>";
        private const double EmoteChance = 0.05;

        GameData gameData = new GameData();
        DiscordSocketClient client;
        SocketTextChannel generalChannel;
        SocketTextChannel testChannel;
        Random random = new Random();
        SocketGuild guild;
        Dictionary<string, BaseCommand> nameToCommand = new Dictionary<string, BaseCommand>();
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        async Task MainAsync()
        {
            AddCommands();
            client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += MessageReceived;
            client.Ready += Ready;
            await client.LoginAsync(TokenType.Bot, Secret.Token);
            await client.StartAsync();
            await gameData.IsGameDone.Task;
            await testChannel.SendMessageAsync($"J13 now offline.");
        }

        private void AddCommands()
        {
            var commands = new List<BaseCommand>()
            {
                new PingCommand(),
                new PongCommand(),
                new HelpCommand(),
                new ExitCommand(),
                new AttackCommand()
            };

            foreach (var command in commands)
            {
                command.Init(gameData);
                nameToCommand.Add(command.Name, command);
            }
        }

        private async Task Ready()
        {
            generalChannel = (SocketTextChannel)client.GetChannel(GeneralId);
            testChannel = (SocketTextChannel)client.GetChannel(TestId);
            guild = client.GetGuild(GuildId);
            int usersCount = guild.Users.Count;
            foreach (var socketUser in guild.Users)
            {
                gameData.IdToPlayer.Add(socketUser.Id, new Player());
            }
            await testChannel.SendMessageAsync($"Deployment to the cloud successful. {usersCount} users detected.");
        }

        Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task MessageReceived(SocketMessage socketMessage)
        {
            if (socketMessage.Author.IsBot)
            {
                return Task.CompletedTask;
            }

            SocketUserMessage socketUserMessage = socketMessage as SocketUserMessage;
            if (socketUserMessage == null)
            {
                return Task.CompletedTask;
            }

            string[] words = socketMessage.Content.Split(' ');
            if (words.Length > 4)
            {
                return Task.CompletedTask;
            }

            List<string> stringParams = null;
            BaseCommand invokedCommand = null;
            foreach (var word in words)
            {
                if (invokedCommand == null)
                {
                    foreach (var command in nameToCommand.Keys)
                    {
                        if (word.ToLowerInvariant() == command)
                        {
                            invokedCommand = nameToCommand[command];
                            stringParams = new List<string>();
                        }
                    }
                }
                else
                {
                    stringParams.Add(word);
                }
            }

            invokedCommand?.OnCommand(stringParams, socketUserMessage);

            if (random.NextDouble() < EmoteChance)
            {
                Emote.TryParse(thinkingEmote, out Emote meigud);
                if (meigud != null)
                {
                socketUserMessage.AddReactionAsync(meigud);
                }
            }
            return Task.CompletedTask;
        }
    }
}
