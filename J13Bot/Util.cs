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
    }
}
