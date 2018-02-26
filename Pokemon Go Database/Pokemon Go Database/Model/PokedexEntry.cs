using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
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
        private int _Number; //Pokemon number
        private String _Species; //Pokemon species name
        private Type _Type1; //Pokemon's first type
        private Type _Type2; //Pokemon's second type
        private MyObservableCollection<PokedexFastMoveWrapper> _FastMoves; //Pokemon's fast moves
        private MyObservableCollection<PokedexChargeMoveWrapper> _ChargeMoves; //Pokemon's charge moves
        private MyObservableCollection<Moveset> _Movesets; //Pokemon's moveset
        private int _Attack; //Pokemon's attack stat
        private int _Defense; //Pokemon's defense stat
        private int _Stamina; //Pokemon's stamina stat

        public PokedexEntry()
        {
            Number = 0;
            Species = "New Pokemon";
            Type1 = Type.None;
            Type2 = Type.None;
            FastMoves = new MyObservableCollection<PokedexFastMoveWrapper>();
            ChargeMoves = new MyObservableCollection<PokedexChargeMoveWrapper>();
            Movesets = new MyObservableCollection<Moveset>();
            Attack = 0;
            Defense = 0;
            Stamina = 0;

            FastMoves.CollectionChanged += FastMovesChanged;
            ChargeMoves.CollectionChanged += ChargeMovesChanged;
        }

        public PokedexEntry(int number, String species = "New Pokemon", Type type1 = Type.None, Type type2 = Type.None)
        {
            Number = number;
            Species = species;
            Type1 = type1;
            Type2 = type2;
            FastMoves = new MyObservableCollection<PokedexFastMoveWrapper>();
            ChargeMoves = new MyObservableCollection<PokedexChargeMoveWrapper>();
            Movesets = new MyObservableCollection<Moveset>();
            Attack = 0;
            Defense = 0;
            Stamina = 0;

            FastMoves.CollectionChanged += FastMovesChanged;
            ChargeMoves.CollectionChanged += ChargeMovesChanged;
        }

        private void ChargeMovesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    List<Moveset> toRemove = new List<Moveset>();
                    foreach (Moveset moveset in _Movesets)
                    {
                        if (moveset.ChargeMove == item as PokedexChargeMoveWrapper)
                        {
                            toRemove.Add(moveset);
                        }
                    }
                    foreach (Moveset moveset in toRemove)
                        Movesets.Remove(moveset);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    foreach (PokedexFastMoveWrapper fastMove in FastMoves)
                    {
                        Movesets.Add(new Moveset(fastMove, item as PokedexChargeMoveWrapper));
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
                    List<Moveset> toRemove = new List<Moveset>();
                    foreach (Moveset moveset in _Movesets)
                    {
                        if (moveset.FastMove == item as PokedexFastMoveWrapper)
                        {
                            Movesets.Remove(moveset);
                        }
                    }
                    foreach (Moveset moveset in toRemove)
                        Movesets.Remove(moveset);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    foreach (PokedexChargeMoveWrapper chargeMove in ChargeMoves)
                    {
                        Movesets.Add(new Moveset(item as PokedexFastMoveWrapper, chargeMove));
                    }
                }
            }
        }

        #region Public Properties

        public int Number
        {
            get
            {
                return _Number;
            }
            set
            {
                _Number = value;
                RaisePropertyChanged("Number");
            }
        }

        public String Species
        {
            get
            {
                return String.Copy(_Species);
            }
            set
            {
                _Species = value;
                RaisePropertyChanged("Species");
            }
        }

        public Type Type1
        {
            get
            {
                return _Type1;
            }
            set
            {
                _Type1 = value;
                RaisePropertyChanged("Type1");
            }
        }

        public Type Type2
        {
            get
            {
                return _Type2;
            }
            set
            {
                _Type2 = value;
                RaisePropertyChanged("Type2");
            }
        }

        public MyObservableCollection<PokedexFastMoveWrapper> FastMoves
        {
            get
            {
                return _FastMoves;
            }
            set
            {
                _FastMoves = value;
                RaisePropertyChanged("FastMoves");
            }
        }
        public MyObservableCollection<PokedexChargeMoveWrapper> ChargeMoves
        {
            get
            {
                return _ChargeMoves;
            }
            set
            {
                _ChargeMoves = value;
                RaisePropertyChanged("ChargeMoves");
            }
        }
        [XmlIgnore]
        public MyObservableCollection<Moveset> Movesets
        {
            get
            {
                return _Movesets;
            }
            private set
            {
                Set(ref this._Movesets, value);
            }
        }
        public int Attack
        {
            get
            {
                return _Attack;
            }
            set
            {
                _Attack = value;
            }
        }
        public int Defense
        {
            get
            {
                return _Defense;
            }
            set
            {
                _Defense = value;
            }
        }
        public int Stamina
        {
            get
            {
                return _Stamina;
            }
            set
            {
                _Stamina = value;
            }
        }

        #region Calculated Properties
        [XmlIgnore]
        public int TotalStats
        {
            get
            {
                return Attack + Defense + Stamina;
            }
        }

        [XmlIgnore]
        public int MaxCP
        {
            get
            {
                double temp1 = (double)Attack + Constants.MaxIV;
                double temp2 = Math.Pow(((double)Stamina + Constants.MaxIV), 0.5);
                double temp3 = Math.Pow(((double)Defense + Constants.MaxIV), 0.5);
                double temp4 = Math.Pow(Constants.CpmValues[Constants.CpmValues.Length - 1], 2);
                return (int)((((double)Attack + Constants.MaxIV) * Math.Pow(((double)Stamina + Constants.MaxIV), 0.5) * Math.Pow(((double)Defense + Constants.MaxIV), 0.5) * Math.Pow(Constants.CpmValues[Constants.CpmValues.Length - 1], 2)) / 10.0);
            }
        }
        #endregion
        #endregion
    }
}