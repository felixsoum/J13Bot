using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class SaveCommand : BaseCommand
    {
        public const string SaveFile = @"..\..\save.data";

        public SaveCommand() : base("save")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (!IsAuthorOwner(message))
            {
                return;
            }

            bool isSaveSuccessful = true;
            try
            {
                var bf = new BinaryFormatter();
                var file = File.Create(SaveFile);
                bf.Serialize(file, gameData.CreateSaveData());
                file.Close();
            }
            catch (Exception e)
            {
                isSaveSuccessful = false;
                message.Channel.SendMessageAsync(Util.FormatEvent(e.Message));
            }

            if (isSaveSuccessful)
            {
                message.Channel.SendMessageAsync(Util.FormatEvent("Successfully saved data."));
            }
        }
    }
}
