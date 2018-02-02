using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Model;
using Pokemon_Go_Database.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pokemon_Go_Database.Popups
{
    public class EditMovesetsViewModel : PopupViewModel
    {
        #region Commands
        public ICommand AddFastMoveCommand { get; private set; }
        public ICommand RemoveFastMoveCommand { get; private set; }
        public ICommand AddChargeMoveCommand { get; private set; }
        public ICommand RemoveChargeMoveCommand { get; private set; }
        public ICommand ExitPopupCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        #endregion
        public EditMovesetsViewModel(SessionService session) : base(session)
        {
            this.AllChargeMoves = this.Session.ChargeMoveList;
            this.AllFastMoves = this.Session.FastMoveList;
            this.Movesets = new ObservableCollection<Moveset>();
            this.FastMoves = new ObservableCollection<MoveWrapper>();
            this.ChargeMoves = new ObservableCollection<MoveWrapper>();

            this.AddFastMoveCommand = new RelayCommand(() => this.FastMoves.Add(new MoveWrapper(this.AllFastMoves[0])));
            this.AddChargeMoveCommand = new RelayCommand(() => this.ChargeMoves.Add(new MoveWrapper(this.AllChargeMoves[0])));

            this.ExitPopupCommand = new RelayCommand(() => Exit());
            this.SaveCommand = new RelayCommand(() => Save());

        }

        public override void Initialize(object param)
        {
            this.Movesets.Clear();
            this.FastMoves.Clear();
            this.ChargeMoves.Clear();
            this.species = param as PokedexEntry;

            if (species != null)
            {
                foreach (FastMove move in species.FastMoves)
                    this.FastMoves.Add(new MoveWrapper(move));
                foreach (ChargeMove move in species.ChargeMoves)
                    this.ChargeMoves.Add(new MoveWrapper(move));
                foreach (MoveWrapper fastMove in this.FastMoves)
                {
                    foreach (MoveWrapper chargeMove in this.ChargeMoves)
                    {
                        this.Movesets.Add(new Moveset(fastMove.Move as FastMove, chargeMove.Move as ChargeMove));
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a valid Pokemon!", "Invalid Pokemon", MessageBoxButton.OK);
                this.ClosePopup(null);
            }
        }

        public override void Deinitialize()
        {
        }

        #region Private Fields
        private bool savingNeeded = false;
        private PokedexEntry species;
        #endregion

        #region Public Properties
        private ObservableCollection<MoveWrapper> _fastMoves;
        public ObservableCollection<MoveWrapper> FastMoves
        {
            get
            {
                return _fastMoves;
            }
            private set
            {
                Set(ref this._fastMoves, value);
            }
        }

        private ObservableCollection<MoveWrapper> _chargeMoves;
        public ObservableCollection<MoveWrapper> ChargeMoves
        {
            get
            {
                return _chargeMoves;
            }
            private set
            {
                Set(ref this._chargeMoves, value);
            }
        }

        private ObservableCollection<Moveset> _movesets;
        public ObservableCollection<Moveset> Movesets
        {
            get
            {
                return _movesets;
            }
            private set
            {
                Set(ref this._movesets, value);
            }
        }

        private ObservableCollection<FastMove> _allFastMoves;
        public ObservableCollection<FastMove> AllFastMoves
        {
            get
            {
                return _allFastMoves;
            }
            private set
            {
                Set(ref this._allFastMoves, value);
            }
        }

        private ObservableCollection<ChargeMove> _allChargeMoves;
        public ObservableCollection<ChargeMove> AllChargeMoves
        {
            get
            {
                return _allChargeMoves;
            }
            private set
            {
                Set(ref this._allChargeMoves, value);
            }
        }
        #endregion

        #region Private Methods
        private void Exit()
        {
            if (this.savingNeeded)
            {
                MessageBoxResult result = MessageBox.Show("Settings have not been saved, are you sure you want to exit?", "Discard Changes?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    this.ClosePopup(null);
            }
            else
            {
                foreach (MoveWrapper move in this.FastMoves)
                    Debug.WriteLine($"Fast move in collection: {move.Move.Name}");
                this.ClosePopup(null);
            }
        }

        private void Save()
        {
            this.species.FastMoves.Clear();
            this.species.ChargeMoves.Clear();
            foreach (MoveWrapper move in this.FastMoves)
                this.species.FastMoves.Add(move.Move as FastMove);
            foreach (MoveWrapper move in this.ChargeMoves)
                this.species.ChargeMoves.Add(move.Move as ChargeMove);
            this.ClosePopup(null);
        }
        #endregion
    }
}
