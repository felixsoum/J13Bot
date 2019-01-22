namespace J13Bot.Game
{
    class Monster
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }

        public Monster(string name, int hp, int gold)
        {
            Name = name;
            Hp = hp;
            Gold = gold;
        }

        public string Damage(int value)
        {
            int preHp = Hp;
            Hp -= value;
            return Util.FormatEvent($"{Name} loses HP {preHp} => {Hp}");
        }
    }
}
