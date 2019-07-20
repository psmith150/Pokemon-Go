using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class IVSet : ObservableObject
    {
        public IVSet(int attackIV = 0, int defenseIV = 0, int staminaIV = 0, double level = 1.0)
        {
            this.AttackIV = attackIV;
            this.DefenseIV = defenseIV;
            this.StaminaIV = staminaIV;
            this.Level = level;
        }
        public IVSet() : this(0)
        {
        }

        #region Public Properties
        private int _AttackIV;
        public int AttackIV
        {
            get
            {
                return this._AttackIV;
            }
            set
            {
                this.Set(ref this._AttackIV, value);
                this.RaisePropertyChanged("IVPercentage");
            }
        }

        private int _DefenseIV;
        public int DefenseIV
        {
            get
            {
                return this._DefenseIV;
            }
            set
            {
                this.Set(ref this._DefenseIV, value);
                this.RaisePropertyChanged("IVPercentage");
            }
        }

        private int _StaminaIV;
        public int StaminaIV
        {
            get
            {
                return this._StaminaIV;
            }
            set
            {
                this.Set(ref this._StaminaIV, value);
                this.RaisePropertyChanged("IVPercentage");
            }
        }

        private double _Level;
        public double Level
        {
            get
            {
                return this._Level;
            }
            set
            {
                this.Set(ref this._Level, value);
            }
        }

        [XmlIgnore]
        public double IVPercentage
        {
            get
            {
                return (this.AttackIV + this.DefenseIV + this.StaminaIV) / (3.0 * Constants.MaxIV);
            }
        }
        #endregion
        #region Public Methods
        public IVSet Copy()
        {
            IVSet copy = new IVSet(this.AttackIV, this.DefenseIV, this.StaminaIV, this.Level);
            return copy;
        }
        #endregion
    }
}
