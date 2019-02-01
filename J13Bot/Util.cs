using System;

namespace J13Bot
{
    static class Util
    {
        public static string FormatEvent(string message)
        {
            string result = "```excel";
            result += "\n";
            result += message;
            result += "\n```";
            return result;
        }

        public static int GetTime()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (int)t.TotalSeconds;
        }
    }
}
