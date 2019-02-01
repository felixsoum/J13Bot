using Discord;
using Discord.WebSocket;
using J13Bot.Commands;
using J13Bot.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        private const double MonsterChance = 0.02;

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
            var commands = new BaseCommand[]
            {
                new PingCommand(),
                new PongCommand(),
                new HelpCommand(),
                new ExitCommand(),
                new AttackCommand(),
                new GiftCommand(),
                new InspectCommand(),
                new OpenCommand(),
                new SaveCommand(),
                new LoadCommand(),
                new QuizCommand(),
                new AnswerCommand(),
                new EatCommand(),
                new ReactCommand(),
                new ImgurCommand(),
                new ChallengeCommand(),
                new DeclineCommand(),
                new PlayCommand()
            };

            foreach (var command in commands)
            {
                command.Init(gameData);
                nameToCommand.Add(command.Name, command);
            }
        }

        private async Task AutoLoad()
        {
            if (File.Exists(SaveCommand.SaveFile))
            {
                FileStream file = null;
                bool errorLoading = false;
                try
                {
                    var bf = new BinaryFormatter();
                    file = File.Open(SaveCommand.SaveFile, FileMode.Open);
                    if (bf.Deserialize(file) is SaveData saveData)
                    {
                        gameData.LoadSaveData(saveData);
                    }
                    else
                    {
                        errorLoading = true;
                        await testChannel.SendMessageAsync($"Save found but wrong format.");
                    }
                    file.Close();
                }
                catch (Exception e)
                {
                    errorLoading = true;
                    await testChannel.SendMessageAsync(Util.FormatEvent(e.Message));
                }
                finally
                {
                    if (file != null)
                    {
                        file.Close();
                    }
                }

                if (errorLoading)
                {
                    File.Delete(SaveCommand.SaveFile);
                }
                else
                {
                    await testChannel.SendMessageAsync(Util.FormatEvent("Auto load data successful."));
                }
            }
            else
            {
                await testChannel.SendMessageAsync("Cannot find save file");
            }
        }

        private async Task Ready()
        {
            generalChannel = (SocketTextChannel)client.GetChannel(GeneralId);
            testChannel = (SocketTextChannel)client.GetChannel(TestId);
            guild = client.GetGuild(GuildId);
            int usersCount = guild.Users.Count;
            await AutoLoad();
            foreach (var socketUser in guild.Users)
            {
                if (gameData.IdToPlayer.ContainsKey(socketUser.Id))
                {
                    gameData.IdToPlayer[socketUser.Id].Username = socketUser.Username;
                }
                else
                {
                    var player = new Player
                    {
                        Id = socketUser.Id,
                        Username = socketUser.Username
                    };
                    gameData.IdToPlayer.Add(socketUser.Id, player);
                }
            }
            await testChannel.SendMessageAsync($"All systems operational (v0.43 - Hotfix Scissors).");
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

            gameData.Tick();

            //if (gameData.IdToPlayer.ContainsKey(socketUserMessage.Author.Id))
            //{
            //    Player messagingPlayer = gameData.IdToPlayer[socketUserMessage.Author.Id];
                //messagingPlayer.Aggro++;

                //var attackTargets = new List<Player>();

                //if (random.NextDouble() < MonsterChance)
                //{
                //    int totalAggro = 0;
                //    foreach (var player in gameData.IdToPlayer.Values)
                //    {
                //        if (player.Aggro > 0)
                //        {
                //            attackTargets.Add(player);
                //            totalAggro += player.Aggro;
                //        }
                //    }

                //    double pickValue = random.NextDouble();
                //    double incrementalChance = 0;
                //    Player attackTarget = attackTargets[attackTargets.Count - 1];
                //    foreach (var player in attackTargets)
                //    {
                //        incrementalChance += (double)player.Aggro / totalAggro;
                //        if (pickValue < incrementalChance)
                //        {
                //            attackTarget = player;
                //            break;
                //        }
                //    }

                //    string reply = "";
                //    if (gameData.ActiveMonster == null)
                //    {
                //        gameData.ActiveMonster = new Monster("Goblin", 30, 5);
                //        reply += $"{gameData.ActiveMonster.Name} has spawned! ";
                //    }

                //    reply += $"{gameData.ActiveMonster.Name} attacks {attackTarget.Username}.";
                //    reply += attackTarget.Damage(10);
                //    attackTarget.Aggro /= 2;
                //    socketUserMessage.Channel.SendMessageAsync(reply);
                //}
            //}

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
