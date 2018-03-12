using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class BattleResult : ObservableObject
    {
        #region Constructor
        public BattleResult()
        {
            this.BattleDuration = 0.0;
            this.NumberOfDeaths = 0;
        }
        #endregion

        #region Public Properties
        private double _BattleDuration;
        public double BattleDuration
        {
            get
            {
                return this._BattleDuration;
            }
            set
            {
                Set(ref this._BattleDuration, value);
            }
        }

        private int _NumberOfDeaths;
        public int NumberOfDeaths
        {
            get
            {
                return this._NumberOfDeaths;
            }
            set
            {
                Set(ref this._NumberOfDeaths, value);
            }
        }
        #endregion
    }
}
