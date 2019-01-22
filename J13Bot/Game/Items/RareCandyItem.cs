namespace J13Bot.Game.Items
{
    class RareCandyItem : BaseItem
    {
        
        public RareCandyItem() : base("RareCandy", ":candy:")
        {
        }

        public override string Eat(string username, Player eater)
        {
            eater.Heal(100);
            eater.Level++;
            return $"{username} levels up after eating a RareCandy!";
        }
    }
}
