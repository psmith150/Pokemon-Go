namespace Pokemon_Go_Database.Model
{
    public class FastMove : Move
    {
        public FastMove() : base()
        {
            base.MoveType = MoveType.Fast;
        }

        public FastMove(string name = "New Move", int power = 0, int time = 1000, int energy = 10, int powerPvp = 0, int energyPvp = 10, int turns = 1,  Type type = Type.None) : base(name, power, time, energy, powerPvp, energyPvp, turns, type)
        {
            base.MoveType = MoveType.Fast;
        }
    }
}
