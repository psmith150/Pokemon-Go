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
            this.IVSets = new MyObservableCollection<IVSet>();
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
        private MyObservableCollection<IVSet> _IVSets;
        public MyObservableCollection<IVSet> IVSets
        {
            get
            {
                return this._IVSets;
            }
            private set
            {
                this.Set(ref this._IVSets, value);
                this.IVSets.CollectionChanged += ((s, a) => this.UpdateAllCalculatedProperties());
            }
        }

        private bool _NeedsFastTM;
        public bool NeedsFastTM
        {
            get
            {
                return this._NeedsFastTM;
            }
            set
            {
                Set(ref this._NeedsFastTM, value);
            }
        }
        private bool _NeedsChargeTM;
        public bool NeedsChargeTM
        {
            get
            {
                return this._NeedsChargeTM;
            }
            set
            {
                Set(ref this._NeedsChargeTM, value);
            }
        }
        private bool _ShouldBePoweredUp;
        public bool ShouldBePoweredUp
        {
            get
            {
                return this._ShouldBePoweredUp;
            }
            set
            {
                Set(ref this._ShouldBePoweredUp, value);
            }
        }
        private bool _IsFavorite;
        public bool IsFavorite
        {
            get
            {
                return this._IsFavorite;
            }
            set
            {
                Set(ref this._IsFavorite, value);
            }
        }
        [XmlIgnore]
        private bool _Compare;
        public bool Compare
        {
            get
            {
                return this._Compare;
            }
            set
            {
                Set(ref this._Compare, value);
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
        public string AttackIVExpression
        {
            get
            {
                return string.Join("/", this.IVSets.Select(x => x.AttackIV).Distinct().OrderBy(x => x).ToList());
            }
            set
            {
                //TODO
                RaisePropertyChanged("Attack");
                RaisePropertyChanged("ActualCP");
                RaisePropertyChanged("IVPercentage");
                RaisePropertyChanged("MaxCP");
            }
        }
        [XmlIgnore]
        public string DefenseIVExpression
        {
            get
            {
                return string.Join("/", this.IVSets.Select(x => x.DefenseIV).Distinct().OrderBy(x => x).ToList());
            }
            set
            {
                //TODO
                RaisePropertyChanged("Defense");
                RaisePropertyChanged("ActualCP");
                RaisePropertyChanged("IVPercentage");
                RaisePropertyChanged("MaxCP");
            }
        }
        [XmlIgnore]
        public string StaminaIVExpression
        {
            get
            {
                return string.Join("/", this.IVSets.Select(x => x.StaminaIV).Distinct().OrderBy(x => x).ToList());
            }
            set
            {
                //TODO
                RaisePropertyChanged("Stamina");
                RaisePropertyChanged("ActualHP");
                RaisePropertyChanged("ActualCP");
                RaisePropertyChanged("IVPercentage");
                RaisePropertyChanged("MaxCP");
            }
        }
        [XmlIgnore]
        public string LevelExpression
        {
            get
            {
                return string.Join("/", this.IVSets.Select(x => x.Level).Distinct().OrderBy(x => x).ToList());
            }
            set
            {
                //TODO
                this.UpdateAllCalculatedProperties();
            }
        }

        [XmlIgnore]
        public double IVPercentage
        {
            get
            {
                if (this.IVSets.Count <= 0)
                    return 0.0;
                return this.IVSets.Select(x => x.IVPercentage).Average();
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
                double movesetDPS = this.Moveset.GetDPS(this.Species.Attack, this.Species.Type1, this.Species.Type2);
                double maxDPS = -1.0;
                foreach (Moveset moveset in this.Species.Movesets)
                {
                    maxDPS = moveset.GetDPS(this.Species.Attack, this.Species.Type1, this.Species.Type2) > maxDPS ? moveset.GetDPS(this.Species.Attack, this.Species.Type1, this.Species.Type2) : maxDPS;
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
                double movesetDPS = this.Moveset.GetDPS(this.Species.Attack, this.Species.Type1, this.Species.Type2, true);
                double maxDPS = -1.0;
                foreach (Moveset moveset in this.Species.Movesets)
                {
                    maxDPS = moveset.GetDPS(this.Species.Attack, this.Species.Type1, this.Species.Type2, true) > maxDPS ? moveset.GetDPS(this.Species.Attack, this.Species.Type1, this.Species.Type2, true) : maxDPS;
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
            if (this.IVSets.Count <= 0)
                return 0;
            return (int)this.IVSets.Select(x => x.AttackIV).Average();
        }
        public int GetStaminaIV()
        {
            if (this.IVSets.Count <= 0)
                return 0;
            return (int)this.IVSets.Select(x => x.StaminaIV).Average();
        }
        public int GetDefenseIV()
        {
            if (this.IVSets.Count <= 0)
                return 0;
            return (int)this.IVSets.Select(x => x.DefenseIV).Average();
        }
        public double GetLevel()
        {
            if (this.IVSets.Count <= 0)
                return 1.0;
            return Math.Round((this.IVSets.Select(x => x.Level).Average() * 2.0)) / 2.0; //Used to round to the nearest 1/2
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
                if (this.IVSets.Count > 0)
                    attackIV = this.IVSets[0].AttackIV;
                else
                    attackIV = 0;
            }
            if (defenseIV == -1)
            {
                if (this.IVSets.Count > 0)
                    defenseIV = this.IVSets[0].DefenseIV;
                else
                    defenseIV = 0;
            }
            if (staminaIV == -1)
            {
                if (this.IVSets.Count > 0)
                    staminaIV = this.IVSets[0].StaminaIV;
                else
                    staminaIV = 0;
            }
            if (level <= 0.0)
            {
                if (this.IVSets.Count > 0)
                    level = this.IVSets[0].Level;
                else
                    level = 1.0;
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
            copy.IVSets = new MyObservableCollection<IVSet>();
            copy.IVSets.InsertRange(this.IVSets);

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
            RaisePropertyChanged("AttackIVExpression");
            RaisePropertyChanged("Defense");
            RaisePropertyChanged("DefenseIVExpression");
            RaisePropertyChanged("Stamina");
            RaisePropertyChanged("StaminaIVExpression");
            RaisePropertyChanged("Level");
            RaisePropertyChanged("LevelExpression");
            RaisePropertyChanged("MaxCP");
            RaisePropertyChanged("IVPercentage");

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