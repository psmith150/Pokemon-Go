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
            this.AttackIVExpression = "0";
            this.DefenseIVExpression = "0";
            this.StaminaIVExpression = "0";
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
                return _Species; ;
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
                UpdateAllCalculatedProperties();
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
                UpdateAllCalculatedProperties();
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
        public Moveset Moveset
        {
            get
            {
                return this.Species.Movesets.SingleOrDefault(x => x.FastMove == this.FastMove && x.ChargeMove == this.ChargeMove);
            }
        }
        [XmlIgnore]
        public string FullName
        {
            get
            {
                return $"{this.Name} - {this.Moveset.Name} - {this.GetAttackIV()}/{this.GetDefenseIV()}/{this.GetStaminaIV()}";
            }
        }
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
                return this.GetCP(-1, -1, -1, Constants.MaxLevel);
            }
        }

        [XmlIgnore]
        public double OffenseMovesetPercentage
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                double movesetDPS = this.Moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2);
                double maxDPS = -1.0;
                foreach (Moveset moveset in this.Species.Movesets)
                {
                    maxDPS = moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2) > maxDPS ? moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2) : maxDPS;
                }
                return movesetDPS / maxDPS;
            }
        }
        [XmlIgnore]
        public double OffenseDPS
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.Moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2);
            }
        }
        private float _offenseDPSPercent;
        [XmlIgnore]
        public double OffenseDPSAtMaxLevel
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.Moveset.GetDPS(this.GetAttack(-1, Constants.MaxLevel), this.Species.Type1, this.Species.Type2);
            }
        }
        private float _offenseDPSPercentAtMax;
        [XmlIgnore]
        public double OffenseTotalDamage
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.OffenseDPS * this.TimeToDeath;
            }
        }
        [XmlIgnore]
        public double OffenseTotalDamageAtMaxLevel
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.OffenseDPSAtMaxLevel * this.TimeToDeathAtMaxLevel;
            }
        }
        [XmlIgnore]
        public double DefenseMovesetPercentage
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                double movesetDPS = this.Moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2, true);
                double maxDPS = -1.0;
                foreach (Moveset moveset in this.Species.Movesets)
                {
                    maxDPS = moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2, true) > maxDPS ? moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2, true) : maxDPS;
                }
                return movesetDPS / maxDPS;
            }
        }
        [XmlIgnore]
        public double DefenseDPS
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.Moveset.GetDPS(this.GetAttack(), this.Species.Type1, this.Species.Type2, true);
            }
        }
        private float _defenseDPSPercent;
        [XmlIgnore]
        public double DefenseDPSAtMaxLevel
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.Moveset.GetDPS(this.GetAttack(-1, Constants.MaxLevel), this.Species.Type1, this.Species.Type2, true);
            }
        }
        private float _defenseDPSPercentAtMax;
        [XmlIgnore]
        public double DefenseTotalDamage
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.DefenseDPS * this.TimeToDeath * Constants.DefenseHPBonus;
            }
        }
        [XmlIgnore]
        public double DefenseTotalDamageAtMaxLevel
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.DefenseDPSAtMaxLevel * this.TimeToDeathAtMaxLevel * Constants.DefenseHPBonus;
            }
        }

        [XmlIgnore]
        public double TimeToDeath
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.GetStamina() / (Math.Floor(0.5 * Constants.IncomingAttack / this.GetDefense() * Constants.IncomingPower) + 1);
            }
        }
        [XmlIgnore]
        public double TimeToDeathAtMaxLevel
        {
            get
            {
                if (this.Moveset == null)
                    return 0.0;
                return this.GetStamina(-1, Constants.MaxLevel) / (Math.Floor(0.5 * Constants.IncomingAttack / this.GetDefense(-1,Constants.MaxLevel) * Constants.IncomingPower) + 1);
            }
        }
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
            return (int)values.Average();
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
            return (int)Math.Truncate((this.GetAttack(attackIV, level) * Math.Pow(this.GetStamina(staminaIV, level), 0.5) * Math.Pow(this.GetDefense(defenseIV, level), 0.5)) / 10.0);

        }

        public Pokemon Copy()
        {
            Pokemon copy = new Pokemon(this.Species, this.Name);
            copy.GameCP = this.GameCP;
            copy.GameHP = this.GameHP;
            copy.DustToPower = this.DustToPower;
            copy.HasBeenPowered = this.HasBeenPowered;
            copy.FastMove = this.FastMove;
            copy.ChargeMove = this.ChargeMove;
            copy.AttackIVExpression = this.AttackIVExpression;
            copy.DefenseIVExpression = this.DefenseIVExpression;
            copy.StaminaIVExpression = this.StaminaIVExpression;
            copy.LevelExpression = this.LevelExpression;

            return copy;
        }
        #endregion

        #region Private Methods
        private void UpdateAllCalculatedProperties()
        {
            RaisePropertyChanged("ActualCP");
            RaisePropertyChanged("ActualHP");
            RaisePropertyChanged("CPM");
            RaisePropertyChanged("Attack");
            RaisePropertyChanged("Defense");
            RaisePropertyChanged("Stamina");
            RaisePropertyChanged("Level");
            RaisePropertyChanged("MaxCP");

            RaisePropertyChanged("OffenseMovesetPercentage");
            RaisePropertyChanged("OffenseDPS");
            RaisePropertyChanged("OffenseDPSAtMaxLevel");
            RaisePropertyChanged("OffenseTotalDamage");
            RaisePropertyChanged("OffenseTotalDamageAtMaxLevel");

            RaisePropertyChanged("DefenseMovesetPercentage");
            RaisePropertyChanged("DefenseDPS");
            RaisePropertyChanged("DefenseDPSAtMaxLevel");
            RaisePropertyChanged("DefenseTotalDamage");
            RaisePropertyChanged("DefenseTotalDamageAtMaxLevel");

            RaisePropertyChanged("TimeToDeath");
            RaisePropertyChanged("TimeToDeathAtMaxLevel");
            //TODO
        }
        #endregion
    }
}