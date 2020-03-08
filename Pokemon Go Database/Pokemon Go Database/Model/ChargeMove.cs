namespace Pokemon_Go_Database.Model
{
    public class ChargeMove : Move
    {
        public ChargeMove() : base()
        {
            base.MoveType = MoveType.Charge;
        }

        public ChargeMove(string name = "New Move", int power = 0, int time = 1000, int energy = 10, int powerPvp = 0, int energyPvp = 10, int turns = 0, Type type = Type.None) : base(name, power, time, energy, powerPvp, energyPvp, turns, type)
        {
            base.MoveType = MoveType.Charge;
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

        public float Dpe
        {
            get
            {
                if (this.EnergyPvp <= 0)
                    return 0;
                return this.PowerPvp / ((float)this.EnergyPvp);
            }
        }

        private int _selfBuff;
        public int SelfBuff
        {
            get
            {
                return _selfBuff;
            }
            set
            {
                Set(ref this._selfBuff, value);
                RaisePropertyChanged("HasSelfBuff");
            }
        }

        public bool HasSelfBuff
        {
            get
            {
                return this.SelfBuff != 0;
            }
        }

        private int _enemyBuff;
        public int EnemyBuff
        {
            get
            {
                return this._enemyBuff;
            }
            set
            {
                Set(ref this._enemyBuff, value);
                RaisePropertyChanged("HasEnemyBuff");
            }
        }

        public bool HasEnemyBuff
        {
            get
            {
                return this.EnemyBuff != 0;
            }
        }

        private double _buffProc;
        public double BuffProc
        {
            get
            {
                return this._buffProc;
            }
            set
            {
                Set(ref this._buffProc, value);
            }
        }
    }
}