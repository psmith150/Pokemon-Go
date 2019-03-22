using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.NumberOfDeaths = 0.0;
            this.DPS = 0.0;
            this.TDO = 0.0;
            this.Name = "New Pokemon";
            this.NextBreakpoint = 0.0;
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

        private double _NumberOfDeaths;
        public double NumberOfDeaths
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

        private double _DPS;
        public double DPS
        {
            get
            {
                return this._DPS;
            }
            set
            {
                Set(ref this._DPS, value);
            }
        }

        private double _TDO;
        public double TDO
        {
            get
            {
                return this._TDO;
            }
            set
            {
                Set(ref this._TDO, value);
            }
        }

        private double _CumulativeDuration;
        public double CumulativeDuration
        {
            get
            {
                return this._CumulativeDuration;
            }
            set
            {
                this.Set(ref this._CumulativeDuration, value);
            }
        }
        private double _CumulativeDPS;
        public double CumulativeDPS
        {
            get
            {
                return this._CumulativeDPS;
            }
            set
            {
                this.Set(ref this._CumulativeDPS, value);
            }
        }
        private double _CumulativeTDO;
        public double CumulativeTDO
        {
            get
            {
                return this._CumulativeTDO;
            }
            set
            {
                this.Set(ref this._CumulativeTDO, value);
            }
        }
        private string _Name;
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.Set(ref this._Name, value);
            }
        }

        private double _NextBreakpoint;
        public double NextBreakpoint
        {
            get
            {
                return this._NextBreakpoint;
            }
            set
            {
                this.Set(ref this._NextBreakpoint, value);
            }
        }
        #endregion
    }
}
