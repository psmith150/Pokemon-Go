using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class Pokemon : ObservableObject
    {

        public Pokemon(string title)
        {
        }

        #region Public Properties
        private string _name;
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                this.Set(ref this._name, value);
            }
        }
        private PokedexEntry _species;
        public PokedexEntry Species
        {
            get
            {
                return _species;;
            }
            set
            {
                Set(ref _species, value);
            }
        }

        private float _actualCP;
        public float ActualCP
        {
            get
            {
                return _actualCP;
            }
            set
            {
                Set(ref _actualCP, value);
            }
        }

        private float _actualHP;
        public float ActualHP
        {
            get
            {
                return _actualHP;
            }
            set
            {
                Set(ref _actualHP, value);
            }
        }

        private int _gameCP;
        public int GameCP
        {
            get
            {
                return _gameCP;
            }
            set
            {
                Set(ref _gameCP, value);
            }
        }

        private int _gameHP;
        public int GameHP
        {
            get
            {
                return _gameHP;
            }
            set
            {
                Set(ref _gameHP, value);
            }
        }

        private int _dustToPower;
        public int DustToPower
        {
            get
            {
                return _dustToPower;
            }
            set
            {
                Set(ref _dustToPower, value);
            }
        }

        private bool _hasBeenPowered;
        public bool HasBeenPowered
        {
            get
            {
                return _hasBeenPowered;
            }
            set
            {
                Set(ref _hasBeenPowered, value);
            }
        }

        private FastMove _fastMove;
        public FastMove FastMove
        {
            get
            {
                return _fastMove;
            }
            set
            {
                Set(ref _fastMove, value);
            }
        }

        private ChargeMove _chargeMove;
        public ChargeMove ChargeMove
        {
            get
            {
                return _chargeMove;
            }
            set
            {
                Set(ref _chargeMove, value);
            }
        }

        private int _attackIV;
        public int AttackIV
        {
            get
            {
                return _attackIV;
            }
            set
            {
                Set(ref _attackIV, value);
            }
        }

        private int _defenseIV;
        public int DefenseIV
        {
            get
            {
                return _defenseIV;
            }
            set
            {
                Set(ref _defenseIV, value);
            }
        }

        private int _staminaIV;
        public int StaminaIV
        {
            get
            {
                return _staminaIV;
            }
            set
            {
                Set(ref _staminaIV, value);
            }
        }

        private int _level;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                Set(ref _level, value);
            }
        }

        private float _ivPercentage;
        [XmlIgnore]
        public float IVPercentage
        {
            get
            {
                return _ivPercentage;
            }
            private set
            {
                Set(ref _ivPercentage, value);
            }
        }

        private float _attack;
        [XmlIgnore]
        public float Attack
        {
            get
            {
                return _attack;
            }
            private set
            {
                Set(ref _attack, value);
            }
        }

        private float _defense;
        [XmlIgnore]
        public float Defense
        {
            get
            {
                return _defense;
            }
            private set
            {
                Set(ref _defense, value);
            }
        }

        private float _stamina;
        [XmlIgnore]
        public float Stamina
        {
            get
            {
                return _stamina;
            }
            private set
            {
                Set(ref _stamina, value);
            }
        }

        private int _maxCP;
        [XmlIgnore]
        public int MaxCP
        {
            get
            {
                return _maxCP;
            }
            private set
            {
                Set(ref _maxCP, value);
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

        #region Public Methods
        #endregion



        #endregion
    }
}