using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    [XmlInclude(typeof(FastMove))]
    [XmlInclude(typeof(ChargeMove))]
    public abstract class Move : ObservableObject
    {
        private string _name; //The name of the move
        private int _power; //The power of the move
        private int _time; //The time the move consumes, in ms
        private int _energy; //The energy the move generates or uses
        private int _powerPvp; // The power of the move in PvP
        private int _turns; //The duration of the move, in turns
        private int _energyPvp; //The energy the move generates or uses in PvP
        private Type _type; //The type of the move
        private MoveType _moveType; //Fast or charge move

        public Move() : this("New Move")
        {
        }

        public Move(string name="New Move", int power=0, int time=1000, int energy = 10, int powerPvp=0, int energyPvp=10, int turns = 1, Type type=Type.None)
        {
            this.Name = name;
            this.Power = power;
            this.Time = time;
            this.Energy = energy;
            this.Type = type;
            this.PowerPvp = powerPvp;
            this.Turns = turns;
            this.EnergyPvp = energy;
            this.Type = type;
            this.DamageWindowStartTime = 0;
            this.DamageWindowDuration = 0;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set(ref this._name, value);
            }
        }

        public int Power
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
                RaisePropertyChanged("Power");
                RaisePropertyChanged("DPS");
            }
        }

        public int Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                RaisePropertyChanged("Time");
                RaisePropertyChanged("DPS");
                RaisePropertyChanged("EPS");
            }
        }

        public int Energy
        {
            get
            {
                return _energy;
            }
            set
            {
                _energy = value;
                RaisePropertyChanged("Energy");
                RaisePropertyChanged("EPS");
            }
        }

        public int PowerPvp
        {
            get
            {
                return _powerPvp;
            }
            set
            {
                Set(ref this._powerPvp, value);
                RaisePropertyChanged("DPT");
            }
        }

        public int Turns
        {
            get
            {
                return _turns;
            }
            set
            {
                Set(ref this._turns, value);
                RaisePropertyChanged("DPT");
                RaisePropertyChanged("EPT");
            }
        }

        public int EnergyPvp
        {
            get
            {
                return _energyPvp;
            }
            set
            {
                Set(ref this._energyPvp, value);
                RaisePropertyChanged("EPT");
            }
        }

        public Type Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }

        public MoveType MoveType
        {
            get
            {
                return _moveType;
            }
            set
            {
                _moveType = value;
                RaisePropertyChanged("MoveType");
            }
        }

        public float DPS
        {
            get
            {
                if (this.Time == 0)
                    return 0;
                return (this.Power / ((float)this.Time / 1000));
            }
        }

        public float EPS
        {
            get
            {
                if (this.Time == 0)
                    return 0;
                return (this.Energy / ((float)this.Time / 1000));
            }
        }

        private int _DamageWindowStartTime;
        public int DamageWindowStartTime
        {
            get
            {
                return this._DamageWindowStartTime;
            }
            set
            {
                Set(ref this._DamageWindowStartTime, value);
            }
        }

        private int _DamageWindowDuration;
        public int DamageWindowDuration
        {
            get
            {
                return this._DamageWindowDuration;
            }
            set
            {
                Set(ref this._DamageWindowDuration, value);
            }
        }

        public float DPT
        {
            get
            {
                if (this.Turns < 0)
                    return 0;
                return (this.PowerPvp / ((float)this.Turns+1));
            }
        }

        public float EPT
        {
            get
            {
                if (this.Turns < 0)
                    return 0;
                return (this.EnergyPvp / ((float)this.Turns+1));
            }
        }

    }

    public class MoveWrapper : ObservableObject
    {
        public MoveWrapper(Move move)
        {
            this.Move = move;
            this.Move.PropertyChanged += ((o, e) => this.RaisePropertyChanged("Move"));
        }

        public MoveWrapper()
        {
        }

        private Move _move;
        public Move Move
        {
            get
            {
                return _move;
            }
            set
            {
                Set(ref this._move, value);
            }
        }
    }
}