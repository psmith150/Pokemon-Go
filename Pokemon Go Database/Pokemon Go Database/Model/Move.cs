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
        private Type _type; //The type of the move
        private MoveType _moveType; //Fast or charge move

        public Move()
        {
            _name = "New Move";
            _power = 0;
            _time = 1000;
            _energy = 10;
            _type = Type.None;
        }

        public Move(string name="New Move", int power=0, int time=1000, int energy=10, Type type=Type.None)
        {
            _name = name;
            _power = power;
            _time = time;
            _energy = energy;
            _type = type;
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
                return (_power / ((float)_time / 1000));
            }
        }

        public float EPS
        {
            get
            {
                return (_energy / ((float)_time / 1000));
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