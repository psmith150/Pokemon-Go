using GalaSoft.MvvmLight;
using System;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class Moveset : ObservableObject
    {
        private Move _fastMove; //The fast move
        private Move _chargeMove; //The charge move

        public Moveset(Move fastMove, Move chargeMove)
        {
            _fastMove = fastMove;
            _chargeMove = chargeMove;
            //_fastMove.PropertyChanged += FastMoveChanged;
            //_chargeMove.PropertyChanged += ChargeMoveChanged;
        }

        private void ChargeMoveChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("ChargeMove");
        }

        private void FastMoveChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("FastMove");
        }

        public Move FastMove
        {
            get
            {
                return _fastMove;
            }
            set
            {
                _fastMove = value;
                RaisePropertyChanged("FastMove");
            }
        }

        public Move ChargeMove
        {
            get
            {
                return _chargeMove;
            }
            set
            {
                _chargeMove = value;
                RaisePropertyChanged("ChargeMove");
            }
        }
    }
}