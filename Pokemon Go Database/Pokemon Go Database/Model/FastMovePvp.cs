namespace Pokemon_Go_Database.Model
{
    public class FastMovePvp : MovePvp
    {
        public FastMovePvp() : base()
        {
            base.MoveType = MoveType.Fast;
        }

        public FastMovePvp(string name = "New Move", int power = 0, int turns = 1, int energy = 10, Type type = Type.None) : base(name, power, turns, energy, type)
        {
            base.MoveType = MoveType.Fast;
        }
    }
}
