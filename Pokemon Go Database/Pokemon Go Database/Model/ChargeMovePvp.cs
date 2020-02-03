namespace Pokemon_Go_Database.Model
{
    public class ChargeMovePvp : MovePvp
    {
        public ChargeMovePvp() : base()
        {
            base.MoveType = MoveType.Charge;
        }

        public ChargeMovePvp(string name = "New Move", int power = 0, int turns = 1, int energy = 10, Type type = Type.None) : base(name, power, turns, energy, type)
        {
            base.MoveType = MoveType.Charge;
        }
    }
}