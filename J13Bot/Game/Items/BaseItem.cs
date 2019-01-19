namespace J13Bot.Game.Items
{
    abstract class BaseItem
    {
        public string Name { get; set; } = "?";
        public string Code { get; set; } = ":question:";

        public BaseItem(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public override string ToString()
        {
            return $"{Code} {Name}";
        }
    }
}
