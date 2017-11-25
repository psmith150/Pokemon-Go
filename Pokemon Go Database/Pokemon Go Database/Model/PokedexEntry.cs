using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Pokemon_Go_Database.Model
{
    /// <summary>
    /// This class stores the data for a single Pokemon species
    /// </summary>
    [Serializable]
    public class PokedexEntry : ObservableObject
    {
        private int _number; //Pokemon number
        private String _species; //Pokemon species name
        private Type _type1; //Pokemon's first type
        private Type _type2; //Pokemon's second type
        private MyObservableCollection<Move> _fastMoves; //Pokemon's fast moves
        private MyObservableCollection<Move> _chargeMoves; //Pokemon's charge moves
        private MyObservableCollection<Moveset> _movesets; //Pokemon's moveset
        private int _attack; //Pokemon's attack stat
        private int _defense; //Pokemon's defense stat
        private int _stamina; //Pokemon's stamina stat

        public PokedexEntry()
        {
            _number = 0;
            _species = "New Pokemon";
            _type1 = Type.None;
            _type2 = Type.None;
            _fastMoves = new MyObservableCollection<Move>();
            _chargeMoves = new MyObservableCollection<Move>();
            _movesets = new MyObservableCollection<Moveset>();
            _attack = 0;
            _defense = 0;
            _stamina = 0;

            _fastMoves.CollectionChanged += FastMovesChanged;
            _chargeMoves.CollectionChanged += ChargeMovesChanged;
        }

        public PokedexEntry(int number, String species="New Pokemon", Type type1=Type.None, Type type2=Type.None)
        {
            _number = number;
            _species = species;
            _type1 = type1;
            _type2 = type2;
            _fastMoves = new MyObservableCollection<Move>();
            _chargeMoves = new MyObservableCollection<Move>();
            _movesets = new MyObservableCollection<Moveset>();
            _attack = 0;
            _defense = 0;
            _stamina = 0;

            _fastMoves.CollectionChanged += FastMovesChanged;
            _chargeMoves.CollectionChanged += ChargeMovesChanged;
        }

        private void ChargeMovesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    foreach (Moveset moveset in _movesets)
                    {
                        if (moveset.ChargeMove == item as Move)
                        {
                            _movesets.Remove(moveset);
                        }
                    }
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    foreach (Move fastMove in _fastMoves)
                    {
                        _movesets.Add(new Moveset(fastMove, item as Move));
                    }
                }
            }
        }

        private void FastMovesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    foreach (Moveset moveset in _movesets)
                    {
                        if (moveset.FastMove == item as Move)
                        {
                            _movesets.Remove(moveset);
                        }
                    }
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    foreach (Move chargeMove in _chargeMoves)
                    {
                        _movesets.Add(new Moveset(item as Move, chargeMove));
                    }
                }
            }
        }

        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                RaisePropertyChanged("Number");
            }
        }

        public String Species
        {
            get
            {
                return String.Copy(_species);
            }
            set
            {
                _species = value;
                RaisePropertyChanged("Species");
            }
        }

        public Type Type1
        {
            get
            {
                return _type1;
            }
            set
            {
                _type1 = value;
                RaisePropertyChanged("Type1");
            }
        }

        public Type Type2
        {
            get
            {
                return _type2;
            }
            set
            {
                _type2 = value;
                RaisePropertyChanged("Type2");
            }
        }

        public MyObservableCollection<Move> FastMoves
        {
            get
            {
                return _fastMoves;
            }
            set
            {
                _fastMoves = value;
                RaisePropertyChanged("FastMoves");
            }
        }
        public MyObservableCollection<Move> ChargeMoves
        {
            get
            {
                return _chargeMoves;
            }
            set
            {
                _chargeMoves = value;
                RaisePropertyChanged("ChargeMoves");
            }
        }
        [XmlIgnore]
        public MyObservableCollection<Moveset> Movesets
        {
            get
            {
                return _movesets;
            }
        }
        public int Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = value;
            }
        }
        public int Defense
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = value;
            }
        }
        public int Stamina
        {
            get
            {
                return _stamina;
            }
            set
            {
                _stamina = value;
            }
        }

    }
}