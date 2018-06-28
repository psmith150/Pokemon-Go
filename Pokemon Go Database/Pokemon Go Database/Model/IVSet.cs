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
        public IVSet()
        {
            this.AttackIV = 0;
            this.DefenseIV = 0;
            this.StaminaIV = 0;
            this.Level = 1;
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
                RaisePropertyChanged("IVPercentage");
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
                RaisePropertyChanged("IVPercentage");
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
                RaisePropertyChanged("IVPercentage");
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
    }
}
