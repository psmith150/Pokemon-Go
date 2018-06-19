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
            this.DamageWindowStartTime = 0;
            this.DamageWindowDuration = 0;
            this.DodgeFlashTime = 0;
        }

        private int _DodgeFlashTime;
        public int DodgeFlashTime
        {
            get
            {
                return this._DodgeFlashTime;
            }
            set
            {
                Set(ref this._DodgeFlashTime, value);
            }
        }
    }
}