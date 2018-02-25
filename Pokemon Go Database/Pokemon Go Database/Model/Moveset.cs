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

        private bool _isLegacy;
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