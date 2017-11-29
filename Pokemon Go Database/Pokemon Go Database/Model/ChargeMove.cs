namespace Pokemon_Go_Database.Model
{
    public class ChargeMove : Move
    {
        public ChargeMove() : base()
        {
            base.MoveType = MoveType.Charge;
        }

        public ChargeMove(string name = "New Move", int power = 0, int time = 1000, int energy = 10, Type type = Type.None) : base(name, power, time, energy, type)
        {
            base.MoveType = MoveType.Charge;
        }
    }
}