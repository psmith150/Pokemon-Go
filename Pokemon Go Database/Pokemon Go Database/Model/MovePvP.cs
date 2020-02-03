using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    [XmlInclude(typeof(FastMovePvp))]
    [XmlInclude(typeof(ChargeMovePvp))]
    public abstract class MovePvp : ObservableObject
    {
        private string _name; //The name of the move
        private int _power; //The power of the move
        private int _turns; //The duration of the move, in turns
        private int _energy; //The energy the move generates or uses
        private Type _type; //The type of the move
        private MoveType _moveType; //Fast or charge move

        public MovePvp() : this("New Move")
        {
        }

        public MovePvp(string name="New Move", int power=0, int turns=1, int energy=10, Type type=Type.None)
        {
            this.Name = name;
            this.Power = power;
            this.Turns = turns;
            this.Energy = energy;
            this.Type = type;
            this.SelfBuff = 0;
            this.EnemyBuff = 0;
            this.BuffProc = 1.0;
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
                Set(ref this._power, value);
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

        public int Energy
        {
            get
            {
                return _energy;
            }
            set
            {
                Set(ref this._energy, value);
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
                Set(ref this._type, value);
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
                Set(ref this._moveType, value);
            }
        }

        public float DPT
        {
            get
            {
                return (_power / (float)_turns);
            }
        }

        public float EPT
        {
            get
            {
                return (_energy / (float)_turns);
            }
        }

        private int _selfBuff;
        public int SelfBuff
        {
            get
            {
                return _selfBuff;
            }
            set
            {
                Set(ref this._selfBuff, value);
                RaisePropertyChanged("HasSelfBuff");
            }
        }

        public bool HasSelfBuff
        {
            get
            {
                return this.SelfBuff != 0;
            }
        }

        private int _enemyBuff;
        public int EnemyBuff
        {
            get
            {
                return this._enemyBuff;
            }
            set
            {
                Set(ref this._enemyBuff, value);
                RaisePropertyChanged("HasEnemyBuff");
            }
        }

        public bool HasEnemyBuff
        {
            get
            {
                return this.EnemyBuff != 0;
            }
        }

        private double _buffProc;
        public double BuffProc
        {
            get
            {
                return this._buffProc;
            }
            set
            {
                Set(ref this._buffProc, value);
            }
        }

    }

    public class MovePvpWrapper : ObservableObject
    {
        public MovePvpWrapper(MovePvp move)
        {
            this.Move = move;
            this.Move.PropertyChanged += ((o, e) => this.RaisePropertyChanged("Move"));
        }

        public MovePvpWrapper()
        {
        }

        private MovePvp _move;
        public MovePvp Move
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