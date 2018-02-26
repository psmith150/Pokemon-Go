using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

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
            this.Level = 1;
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
            }
        }

        private float _ActualCP;
        public float ActualCP
        {
            get
            {
                return _ActualCP;
            }
            set
            {
                Set(ref _ActualCP, value);
            }
        }

        private float _ActualHP;
        public float ActualHP
        {
            get
            {
                return _ActualHP;
            }
            set
            {
                Set(ref _ActualHP, value);
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

        private int _AttackIV;
        public int AttackIV
        {
            get
            {
                return _AttackIV;
            }
            set
            {
                Set(ref _AttackIV, value);
            }
        }

        private int _DefenseIV;
        public int DefenseIV
        {
            get
            {
                return _DefenseIV;
            }
            set
            {
                Set(ref _DefenseIV, value);
            }
        }

        private int _StaminaIV;
        public int StaminaIV
        {
            get
            {
                return _StaminaIV;
            }
            set
            {
                Set(ref _StaminaIV, value);
            }
        }

        private double _Level;
        public double Level
        {
            get
            {
                return _Level;
            }
            set
            {
                Set(ref _Level, value);
            }
        }

        private float _IVPercentage;
        [XmlIgnore]
        public float IVPercentage
        {
            get
            {
                return _IVPercentage;
            }
            private set
            {
                Set(ref _IVPercentage, value);
            }
        }

        private float _Attack;
        [XmlIgnore]
        public float Attack
        {
            get
            {
                return _Attack;
            }
            private set
            {
                Set(ref _Attack, value);
            }
        }

        private float _Defense;
        [XmlIgnore]
        public float Defense
        {
            get
            {
                return _Defense;
            }
            private set
            {
                Set(ref _Defense, value);
            }
        }

        private float _Stamina;
        [XmlIgnore]
        public float Stamina
        {
            get
            {
                return _Stamina;
            }
            private set
            {
                Set(ref _Stamina, value);
            }
        }

        private int _MaxCP;
        [XmlIgnore]
        public int MaxCP
        {
            get
            {
                return _MaxCP;
            }
            private set
            {
                Set(ref _MaxCP, value);
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
    }
}