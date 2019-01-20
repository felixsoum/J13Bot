using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Discord.WebSocket;
using J13Bot.Game;

namespace J13Bot.Commands
{
    class LoadCommand : BaseCommand
    {
        public LoadCommand() : base("load")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (!IsAuthorOwner(message))
            {
                return;
            }

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
                        message.Channel.SendMessageAsync($"Save found but wrong format.");
                    }
                    file.Close();
                }
                catch (Exception e)
                {
                    errorLoading = true;
                    message.Channel.SendMessageAsync(Util.FormatEvent(e.Message));
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
                    message.Channel.SendMessageAsync(Util.FormatEvent("Load data successful."));
                }
            }
            else
            {
                message.Channel.SendMessageAsync("Cannot find save file");
            }
        }
    }
}
