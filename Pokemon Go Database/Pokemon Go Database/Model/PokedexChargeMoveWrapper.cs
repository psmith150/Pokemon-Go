using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class PokedexChargeMoveWrapper : ObservableObject
    {
        #region Constructor
        public PokedexChargeMoveWrapper()
        {
            this.ChargeMove = new ChargeMove();
            this.IsLegacy = false;
        }
        public PokedexChargeMoveWrapper(ChargeMove chargeMove, bool isLegacy = false)
        {
            this.ChargeMove = chargeMove;
            this.IsLegacy = isLegacy;
        }
        #endregion

        #region Public Properties
        private ChargeMove _ChargeMove;
        public ChargeMove ChargeMove
        {
            get
            {
                return _ChargeMove;
            }
            set
            {
                Set(ref this._ChargeMove, value);
            }
        }

        private bool _IsLegacy;
        public bool IsLegacy
        {
            get
            {
                return _IsLegacy;
            }
            set
            {
                Set(ref this._IsLegacy, value);
            }
        }
        #endregion
    }
}
