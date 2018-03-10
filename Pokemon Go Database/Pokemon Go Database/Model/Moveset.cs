using GalaSoft.MvvmLight;
using System;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class Moveset : ObservableObject
    {
        public Moveset(PokedexFastMoveWrapper fastMove, PokedexChargeMoveWrapper chargeMove)
        {
            FastMove = fastMove;
            ChargeMove = chargeMove;
            FastMove.PropertyChanged += FastMoveChanged;
            ChargeMove.PropertyChanged += ChargeMoveChanged;
        }
        #region Public Properties
        private PokedexFastMoveWrapper _fastMove;
        public PokedexFastMoveWrapper FastMove
        {
            get
            {
                return _fastMove;
            }
            set
            {
                this.Set(ref this._fastMove, value);
            }
        }

        private PokedexChargeMoveWrapper _chargeMove;
        public PokedexChargeMoveWrapper ChargeMove
        {
            get
            {
                return _chargeMove;
            }
            set
            {
                this.Set(ref this._chargeMove, value);
            }
        }

        public bool IsLegacy
        {
            get
            {
                return this.ChargeMove.IsLegacy || this.FastMove.IsLegacy;
            }
        }

        public string Name
        {
            get
            {
                return $"{this.FastMove.FastMove.Name}/{this.ChargeMove.ChargeMove.Name}" + (this.IsLegacy ? "*" : "");
            }
        }
        #endregion

        #region Public Methods
        public double GetDPS(double attack, Type type1 = Type.None, Type type2 = Type.None, bool isDefending = false)
        {
            double fastMoveStab = 1.0;
            double chargeMoveStab = 1.0;
            if (type1 == this.FastMove.FastMove.Type || type2 == this.ChargeMove.ChargeMove.Type)
                fastMoveStab = Constants.StabBonus;
            if (type1 == this.ChargeMove.ChargeMove.Type || type2 == this.ChargeMove.ChargeMove.Type)
                chargeMoveStab = Constants.StabBonus;
            int fastMoveDamage = (int)Math.Floor(0.5 * this.FastMove.FastMove.Power * attack * fastMoveStab / Constants.TestDefense) + 1;
            int chargeMoveDamage = (int)Math.Floor(0.5 * this.ChargeMove.ChargeMove.Power * attack * chargeMoveStab / Constants.TestDefense) + 1;
            double defenseDuration = isDefending ? 2000.0 : 0.0;

            return (fastMoveDamage * this.ChargeMove.ChargeMove.Energy + chargeMoveDamage * this.FastMove.FastMove.Energy) / ((this.FastMove.FastMove.Time + defenseDuration) / 1000.0 * this.ChargeMove.ChargeMove.Energy + this.ChargeMove.ChargeMove.Time / 1000.0 * this.FastMove.FastMove.Energy);
        }
        #endregion

        #region Private Methods
        private void ChargeMoveChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("ChargeMove");
            RaisePropertyChanged("Name");
        }

        private void FastMoveChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("FastMove");
            RaisePropertyChanged("Name");
        }
        #endregion

    }
}