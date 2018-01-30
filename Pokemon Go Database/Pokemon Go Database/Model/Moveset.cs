using GalaSoft.MvvmLight;
using System;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class Moveset : ObservableObject
    {
        public Moveset(FastMove fastMove, ChargeMove chargeMove)
        {
            FastMove = fastMove;
            ChargeMove = chargeMove;
            FastMove.PropertyChanged += FastMoveChanged;
            ChargeMove.PropertyChanged += ChargeMoveChanged;
        }
        #region Public Properties
        private FastMove _fastMove;
        public FastMove FastMove
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

        private ChargeMove _chargeMove;
        public ChargeMove ChargeMove
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
                return this._isLegacy;
            }
            set
            {
                this.Set(ref this._isLegacy, value);
            }
        }

        public string Name
        {
            get
            {
                return $"{this.FastMove.Name}/{this.ChargeMove.Name}" + (this.IsLegacy ? "*" : "");
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