using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace J13Bot.Commands
{

    public class Rootobject
    {
        public Datum[] data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int datetime { get; set; }
        public string type { get; set; }
        public bool animated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public int views { get; set; }
        public long bandwidth { get; set; }
        public object vote { get; set; }
        public bool favorite { get; set; }
        public bool nsfw { get; set; }
        public string section { get; set; }
        public string account_url { get; set; }
        public int account_id { get; set; }
        public bool is_ad { get; set; }
        public bool in_most_viral { get; set; }
        public bool has_sound { get; set; }
        public Tag[] tags { get; set; }
        public int ad_type { get; set; }
        public string ad_url { get; set; }
        public bool in_gallery { get; set; }
        public string topic { get; set; }
        public int topic_id { get; set; }
        public string link { get; set; }
        public int comment_count { get; set; }
        public int favorite_count { get; set; }
        public int ups { get; set; }
        public int downs { get; set; }
        public int points { get; set; }
        public int score { get; set; }
        public bool is_album { get; set; }
        public string cover { get; set; }
        public int cover_width { get; set; }
        public int cover_height { get; set; }
        public string privacy { get; set; }
        public string layout { get; set; }
        public int images_count { get; set; }
        public bool include_album_ads { get; set; }
        public Image[] images { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string display_name { get; set; }
        public int followers { get; set; }
        public int total_items { get; set; }
        public bool following { get; set; }
        public string background_hash { get; set; }
        public string thumbnail_hash { get; set; }
        public string accent { get; set; }
        public bool background_is_animated { get; set; }
        public bool thumbnail_is_animated { get; set; }
        public bool is_promoted { get; set; }
        public string description { get; set; }
        public string logo_hash { get; set; }
        public string logo_destination_url { get; set; }
        public Description_Annotations description_annotations { get; set; }
    }

    public class Description_Annotations
    {
    }

    public class Image
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int datetime { get; set; }
        public string type { get; set; }
        public bool animated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public int views { get; set; }
        public long bandwidth { get; set; }
        public object vote { get; set; }
        public bool favorite { get; set; }
        public object nsfw { get; set; }
        public object section { get; set; }
        public object account_url { get; set; }
        public object account_id { get; set; }
        public bool is_ad { get; set; }
        public bool in_most_viral { get; set; }
        public bool has_sound { get; set; }
        public object[] tags { get; set; }
        public int ad_type { get; set; }
        public string ad_url { get; set; }
        public bool in_gallery { get; set; }
        public string link { get; set; }
        public string mp4 { get; set; }
        public string gifv { get; set; }
        public string hls { get; set; }
        public int mp4_size { get; set; }
        public bool looping { get; set; }
        public Processing processing { get; set; }
        public object comment_count { get; set; }
        public object favorite_count { get; set; }
        public object ups { get; set; }
        public object downs { get; set; }
        public object points { get; set; }
        public object score { get; set; }
    }

    public class Processing
    {
        public string status { get; set; }
    }


    class ImgurCommand : BaseCommand
    {
        static Random random = new Random();
        Regex imgurRegex = new Regex("\"id\":\"(.......)", RegexOptions.Compiled);
        static List<string> imgurIds = new List<string>();

        public ImgurCommand() : base("imgur")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            if (stringParams.Count > 0)
            {
                string request = stringParams[0];
                if (request.Length < 20 && request.All(c => Char.IsLetter(c) || c == '+'))
                {
                    message.Channel.SendMessageAsync(RequestToImgUrl(request));
                }
            }
        }

        public static string GetJson(string request)
        {
            string url = @"https://api.imgur.com/3/gallery/search/viral/?q_type=jpg&q_type=png&q=" + request;
            Uri uri = new Uri(url);
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Headers.Add("Authorization", "Client-ID " + Secret.ClientId);
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string output = reader.ReadToEnd();
            response.Close();

            return output;
        }

        public static string RequestToImgUrl(string request)
        {
            string jsonString = GetJson(request);

            imgurIds.Clear();

            var root = JsonConvert.DeserializeObject<Rootobject>(jsonString);
            if (root.data.Length > 0)
            {
                foreach (var data in root.data)
                {
                    if (!data.nsfw)
                    {
                        imgurIds.Add(data.id);
                    }
                }
            }

            if (imgurIds.Count == 0)
            {
                return "";
            }
            string randomId = imgurIds[random.Next(imgurIds.Count)];
            return @"https://imgur.com/" + randomId;
        }
    }
}
