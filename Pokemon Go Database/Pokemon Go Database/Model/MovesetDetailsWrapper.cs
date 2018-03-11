using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class MovesetDetailsWrapper : ObservableObject
    {
        #region Constructor
        public MovesetDetailsWrapper(Moveset moveset, Type type1 = Type.None, Type type2 = Type.None, double attack = 10.0, bool isDefending = false)
        {
            this.Moveset = moveset;
            this.Type1 = type1;
            this.Type2 = type2;
            this.Attack = attack;
            this.IsDefending = IsDefending;
            this.DPSUpperLimit = 1.0;
        }
        #endregion

        #region Public Properties
        private Moveset _Moveset;
        public Moveset Moveset
        {
            get
            {
                return this._Moveset;
            }
            set
            {
                Set(ref this._Moveset, value);
                RaisePropertyChanged("DPS");
                RaisePropertyChanged("DPSPercentage");
            }
        }
        private double _Attack;
        public double Attack
        {
            get
            {
                return this._Attack;
            }
            set
            {
                this._Attack = value;
                RaisePropertyChanged("DPS");
                RaisePropertyChanged("DPSPercentage");
            }
        }
        private bool _IsDefending;
        public bool IsDefending
        {
            get
            {
                return this._IsDefending;
            }
            set
            {
                this._IsDefending = value;
                RaisePropertyChanged("DPS");
                RaisePropertyChanged("DPSPercentage");
            }
        }
        private Type _Type1;
        public Type Type1
        {
            get
            {
                return this._Type1;
            }
            set
            {
                this._Type1 = value;
                RaisePropertyChanged("DPS");
                RaisePropertyChanged("DPSPercentage");
            }
        }
        private Type _Type2;
        public Type Type2
        {
            get
            {
                return this._Type2;
            }
            set
            {
                this._Type2 = value;
                RaisePropertyChanged("DPS");
                RaisePropertyChanged("DPSPercentage");
            }
        }
        private double _DPSUpperLimit;
        public double DPSUpperLimit
        {
            get
            {
                return this._DPSUpperLimit;
            }
            set
            {
                this._DPSUpperLimit = value;
                RaisePropertyChanged("DPS");
                RaisePropertyChanged("DPSPercentage");
            }
        }
        public double DPS
        {
            get
            {
                return this.Moveset.GetDPS(this.Attack, this.Type1, this.Type2, this.IsDefending);
            }
        }
        public double DPSPercentage
        {
            get
            {
                if (this.DPSUpperLimit != 0.0)
                    return this.DPS / this.DPSUpperLimit;
                return 0.0;
            }
        }
        #endregion
    }
}
