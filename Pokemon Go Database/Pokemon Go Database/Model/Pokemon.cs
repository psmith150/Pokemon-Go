using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Diagnostics;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class Pokemon : ObservableObject
    {

        public Pokemon() : this(new PokedexEntry())
        {
        }

        public Pokemon(PokedexEntry species, string name = "New Pokemon")
        {
            this.Name = name;
            this.Species = species;
            this.LevelExpression = "1";
        }

        #region Public Properties
        private string _Name;
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                this.Set(ref this._Name, value);
            }
        }
        private PokedexEntry _Species;
        public PokedexEntry Species
        {
            get
            {
                return _Species;;
            }
            set
            {
                Set(ref _Species, value);
                this.UpdateAllCalculatedProperties();
            }
        }

        private int _GameCP;
        public int GameCP
        {
            get
            {
                return _GameCP;
            }
            set
            {
                Set(ref _GameCP, value);
            }
        }

        private int _GameHP;
        public int GameHP
        {
            get
            {
                return _GameHP;
            }
            set
            {
                Set(ref _GameHP, value);
            }
        }

        private int _DustToPower;
        public int DustToPower
        {
            get
            {
                return _DustToPower;
            }
            set
            {
                Set(ref _DustToPower, value);
            }
        }

        private bool _HasBeenPowered;
        public bool HasBeenPowered
        {
            get
            {
                return _HasBeenPowered;
            }
            set
            {
                Set(ref _HasBeenPowered, value);
            }
        }

        private PokedexFastMoveWrapper _FastMove;
        public PokedexFastMoveWrapper FastMove
        {
            get
            {
                return _FastMove;
            }
            set
            {
                Set(ref _FastMove, value);
            }
        }

        private PokedexChargeMoveWrapper _ChargeMove;
        public PokedexChargeMoveWrapper ChargeMove
        {
            get
            {
                return _ChargeMove;
            }
            set
            {
                Set(ref _ChargeMove, value);
            }
        }

        private string _AttackIVExpression;
        public string AttackIVExpression
        {
            get
            {
                return _AttackIVExpression;
            }
            set
            {
                Set(ref _AttackIVExpression, value);
                RaisePropertyChanged("Attack");
                RaisePropertyChanged("ActualCP");
                RaisePropertyChanged("IVPercentage");
                RaisePropertyChanged("MaxCP");
            }
        }

        private string _DefenseIVExpression;
        public string DefenseIVExpression
        {
            get
            {
                return _DefenseIVExpression;
            }
            set
            {
                Set(ref _DefenseIVExpression, value);
                RaisePropertyChanged("Defense");
                RaisePropertyChanged("ActualCP");
                RaisePropertyChanged("IVPercentage");
                RaisePropertyChanged("MaxCP");
            }
        }

        private string _StaminaIVExpression;
        public string StaminaIVExpression
        {
            get
            {
                return _StaminaIVExpression;
            }
            set
            {
                Set(ref _StaminaIVExpression, value);
                RaisePropertyChanged("Stamina");
                RaisePropertyChanged("ActualHP");
                RaisePropertyChanged("ActualCP");
                RaisePropertyChanged("IVPercentage");
                RaisePropertyChanged("MaxCP");
            }
        }

        private string _LevelExpression;
        public string LevelExpression
        {
            get
            {
                return _LevelExpression;
            }
            set
            {
                Set(ref _LevelExpression, value);
                this.UpdateAllCalculatedProperties();
            }
        }
        #region Calculated Properties
        [XmlIgnore]
        public int ActualCP
        {
            get
            {
                return this.GetCP();
            }
        }

        [XmlIgnore]
        public int ActualHP
        {
            get
            {
                return (int)this.GetStamina();
            }
        }
        [XmlIgnore]
        public double CPM
        {
            get
            {
                return this.GetCpmValue();
            }
        }
        [XmlIgnore]
        public double IVPercentage
        {
            get
            {
                return (GetAttackIV() + GetStaminaIV() + GetDefenseIV()) / (3.0 * Constants.MaxIV);
            }
        }

        [XmlIgnore]
        public int Attack
        {
            get
            {
                return (int)this.GetAttack();
            }
        }

        [XmlIgnore]
        public int Defense
        {
            get
            {
                return (int)this.GetDefense();
            }
        }
        [XmlIgnore]
        public int Stamina
        {
            get
            {
                return (int)this.GetStamina();
            }
        }

        [XmlIgnore]
        public double Level
        {
            get
            {
                return this.GetLevel();
            }
        }

        [XmlIgnore]
        public int MaxCP
        {
            get
            {
                return this.GetCP(-1,-1,-1, Constants.MaxLevel);
            }
        }

        private float _offenseMovesetPercent;
        private float _offenseDPS;
        private float _offenseDPSPercent;
        private float _offenseDPSAtMax;
        private float _offenseDPSPercentAtMax;
        private float _timeToDeath;
        private float _timeToDeathAtMax;
        private float _offenseTotalDamage;
        private float _offenseTotalDamageAtMax;
        private float _defenseMovesetPercent;
        private float _defenseDPS;
        private float _defenseDPSPercent;
        private float _defenseDPSAtMax;
        private float _defenseDPSPercentAtMax;
        private float _defenseTotalDamage;
        private float _defenseTotalDamageAtMax;
        #endregion
        #endregion

        #region Public Methods
        public int GetAttackIV()
        {
            List<int> values = new List<int>();
            foreach (string value in this.AttackIVExpression.Split('/'))
            {
                try
                {
                    values.Add(Int32.Parse(value));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Error parsing attack IV expression {this.AttackIVExpression} for pokemon {this.Name}.");
                }
            }
            if (values.Count <= 0)
                return 0;
            return (int) values.Average();
        }
        public int GetStaminaIV()
        {
            List<int> values = new List<int>();
            foreach (string value in this.StaminaIVExpression.Split('/'))
            {
                try
                {
                    values.Add(Int32.Parse(value));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Error parsing stamina IV expression {this.StaminaIVExpression} for pokemon {this.Name}.");
                }
            }
            if (values.Count <= 0)
                return 0;
            return (int)values.Average();
        }
        public int GetDefenseIV()
        {
            List<int> values = new List<int>();
            foreach (string value in this.DefenseIVExpression.Split('/'))
            {
                try
                {
                    values.Add(Int32.Parse(value));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Error parsing defense IV expression {this.DefenseIVExpression} for pokemon {this.Name}.");
                }
            }
            if (values.Count <= 0)
                return 0;
            return (int)values.Average();
        }
        public double GetLevel()
        {
            List<double> values = new List<double>();
            foreach (string value in this.LevelExpression.Split('/'))
            {
                try
                {
                    values.Add(Double.Parse(value));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Error parsing level expression {this.LevelExpression} for pokemon {this.Name}.");
                }
            }
            if (values.Count <= 0)
                return 1.0;
            return Math.Round((values.Average() * 2.0)) / 2.0; //Used to round to the nearest 1/2
        }

        public double GetAttack(int attackIV = -1, double level = -1.0)
        {
            if (attackIV == -1)
            {
                attackIV = this.GetAttackIV();
            }
            return (Species.Attack + attackIV) * GetCpmValue(level);
        }

        public double GetStamina(int staminaIV = -1, double level = -1.0)
        {
            if (staminaIV == -1)
            {
                staminaIV = this.GetStaminaIV();
            }
            return (Species.Stamina + staminaIV) * GetCpmValue(level);
        }

        public double GetDefense(int defenseIV = -1, double level = -1.0)
        {
            if (defenseIV == -1)
            {
                defenseIV = this.GetDefenseIV();
            }
            return (Species.Defense + defenseIV) * GetCpmValue(level);
        }

        public double GetCpmValue(double level = -1.0)
        {
            if (level <= 0.0)
            {
                level = this.GetLevel();
            }
            return Constants.CpmValues[(int)(level * 2 - 2)];
        }

        public int GetCP(int attackIV = -1, int staminaIV = -1, int defenseIV = -1, double level = -1.0)
        {
            if (attackIV == -1)
            {
                attackIV = this.GetAttackIV();
            }
            if (defenseIV == -1)
            {
                defenseIV = this.GetDefenseIV();
            }
            if (staminaIV == -1)
            {
                staminaIV = this.GetStaminaIV();
            }
            if (level <= 0.0)
            {
                level = this.GetLevel();
            }
            return (int) Math.Truncate((this.GetAttack(attackIV, level) * Math.Pow(this.GetStamina(staminaIV, level), 0.5) * Math.Pow(this.GetDefense(defenseIV, level), 0.5)) / 10.0);

        }
        #endregion

        #region Private Methods
        private void UpdateAllCalculatedProperties()
        {
            RaisePropertyChanged("Attack");
            RaisePropertyChanged("Defense");
            RaisePropertyChanged("Stamina");
            RaisePropertyChanged("Attack");
            RaisePropertyChanged("Level");
            RaisePropertyChanged("ActualCP");
            RaisePropertyChanged("ActualHP");
            RaisePropertyChanged("MaxCP");
            RaisePropertyChanged("CPM");
            //TODO
        }
        #endregion
    }
}