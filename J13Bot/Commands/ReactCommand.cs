using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class ReactCommand : BaseCommand
    {
        Random random = new Random();
        Regex gfyregex = new Regex("gfyName\":\"(\\w*)\"", RegexOptions.Compiled);
        List<string> gfyNames = new List<string>();

        public ReactCommand() : base("react")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (stringParams.Count > 0)
            {
                string request = stringParams[0];
                if (request.Length < 20 && request.All(c => Char.IsLetter(c) || c == '+'))
                {
                    string jsonString = GetJson(@"https://api.gfycat.com/v1/gfycats/search?search_text=" + request);

                    MatchCollection matches = gfyregex.Matches(jsonString);
                    if (matches == null || matches.Count == 0)
                    {
                        return;
                    }

                    gfyNames.Clear();
                    foreach (Match match in matches)
                    {
                        if (match.Groups.Count > 1)
                        {
                            gfyNames.Add(match.Groups[1].Value);
                        }
                    }

                    if (gfyNames.Count == 0)
                    {
                        return;
                    }
                    string randomName = gfyNames[random.Next(gfyNames.Count)];
                    string reply = @"https://gfycat.com/";
                    reply += randomName;
                    message.Channel.SendMessageAsync(reply);
                }
            }
        }

        public string GetJson(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string output = reader.ReadToEnd();
            response.Close();

            return output;
        }
    }
}
