using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Base.AbstractClasses;
using Pokemon_Go_Database.Base.Enums;
using Pokemon_Go_Database.Base.EventArgs;
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
using System.Windows.Data;
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
        public ICommand ShowEditMovesetsCommand { get; private set; }
        public ICommand ShowOffenseDetailsCommand { get; private set; }
        public ICommand ShowDefenseDetailsCommand { get; private set; }
        #endregion

        public EditMovesetsViewModel(SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this._messageViewer = messageViewer;
            this.AllChargeMoves = this.Session.ChargeMoveList;
            this.AllFastMoves = this.Session.FastMoveList;
            this.Movesets = new ObservableCollection<MovesetDetailsWrapper>();
            this.FastMoves = new ObservableCollection<PokedexFastMoveWrapper>();
            this.ChargeMoves = new ObservableCollection<PokedexChargeMoveWrapper>();

            this.FastMoves.CollectionChanged += FastMoves_CollectionChanged;
            this.ChargeMoves.CollectionChanged += ChargeMoves_CollectionChanged;

            this.AddFastMoveCommand = new RelayCommand(() => this.FastMoves.Add(new PokedexFastMoveWrapper(this.AllFastMoves[0])));
            this.AddChargeMoveCommand = new RelayCommand(() => this.ChargeMoves.Add(new PokedexChargeMoveWrapper(this.AllChargeMoves[0])));

            this.RemoveFastMoveCommand = new RelayCommand(() => RemoveFastMove());
            this.RemoveChargeMoveCommand = new RelayCommand(() => RemoveChargeMove());

            this.ExitPopupCommand = new RelayCommand(() => Exit());
            this.SaveCommand = new RelayCommand(() => Save());

            this.ShowEditMovesetsCommand = new RelayCommand(() => { this.EditMovesetsVisible = true; this.OffenseDetailsVisible = false; this.DefenseDetailsVisible = false; });
            this.ShowOffenseDetailsCommand = new RelayCommand(() => ShowOffenseDetails());
            this.ShowDefenseDetailsCommand = new RelayCommand(() => ShowDefenseDetails());

        }

        
        public override void Initialize(object param)
        {
            this.Movesets.Clear();
            this.FastMoves.Clear();
            this.ChargeMoves.Clear();
            this.Species = param as PokedexEntry;
            this.savingNeeded = false;

            if (this.Species != null)
            {
                foreach (PokedexFastMoveWrapper move in this.Species.FastMoves)
                    this.FastMoves.Add(move);
                foreach (PokedexChargeMoveWrapper move in this.Species.ChargeMoves)
                    this.ChargeMoves.Add(move);
            }
            else
            {
                this._messageViewer.DisplayMessage("Please select a valid Pokemon!", "Invalid Pokemon", MessageViewerButton.Ok).Wait();
                this.ClosePopup(null);
            }
            this.EditMovesetsVisible = true;
            this.OffenseDetailsVisible = false;
            this.DefenseDetailsVisible = false;
            this.FastMoves.CollectionChanged += this.MoveCollectionChanged;
            this.ChargeMoves.CollectionChanged += this.MoveCollectionChanged;
        }

        public override void Deinitialize()
        {
            this.FastMoves.CollectionChanged -= this.MoveCollectionChanged;
            this.ChargeMoves.CollectionChanged -= this.MoveCollectionChanged;
        }

        #region Private Fields
        private bool savingNeeded = false;
        private PokedexEntry _species;
        private MessageViewerBase _messageViewer;
        #endregion

        #region Public Properties
        public PokedexEntry Species
        {
            get
            {
                return _species;
            }
            private set
            {
                Set(ref this._species, value);
            }
        }
        private ObservableCollection<PokedexFastMoveWrapper> _FastMoves;
        public ObservableCollection<PokedexFastMoveWrapper> FastMoves
        {
            get
            {
                return _FastMoves;
            }
            private set
            {
                Set(ref this._FastMoves, value);
            }
        }

        private ObservableCollection<PokedexChargeMoveWrapper> _ChargeMoves;
        public ObservableCollection<PokedexChargeMoveWrapper> ChargeMoves
        {
            get
            {
                return _ChargeMoves;
            }
            private set
            {
                Set(ref this._ChargeMoves, value);
            }
        }
        private ListCollectionView _MovesetsView;
        public ListCollectionView MovesetsView
        {
            get
            {
                return this._MovesetsView;
            }
            private set
            {
                Set(ref this._MovesetsView, value);
                this.MovesetsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("DPS", System.ComponentModel.ListSortDirection.Descending));
            }
        }
        private ObservableCollection<MovesetDetailsWrapper> _Movesets;
        public ObservableCollection<MovesetDetailsWrapper> Movesets
        {
            get
            {
                return _Movesets;
            }
            private set
            {
                Set(ref this._Movesets, value);
                this.MovesetsView = new ListCollectionView(this.Movesets);
            }
        }

        private ObservableCollection<FastMove> _AllFastMoves;
        public ObservableCollection<FastMove> AllFastMoves
        {
            get
            {
                return _AllFastMoves;
            }
            private set
            {
                Set(ref this._AllFastMoves, value);
            }
        }

        private ObservableCollection<ChargeMove> _AllChargeMoves;
        public ObservableCollection<ChargeMove> AllChargeMoves
        {
            get
            {
                return _AllChargeMoves;
            }
            private set
            {
                Set(ref this._AllChargeMoves, value);
            }
        }

        private PokedexFastMoveWrapper _SelectedFastMove;
        public PokedexFastMoveWrapper SelectedFastMove
        {
            get
            {
                return _SelectedFastMove;
            }
            set
            {
                Set(ref this._SelectedFastMove, value);
            }
        }

        private PokedexChargeMoveWrapper _SelectedChargeMove;
        public PokedexChargeMoveWrapper SelectedChargeMove
        {
            get
            {
                return _SelectedChargeMove;
            }
            set
            {
                Set(ref this._SelectedChargeMove, value);
            }
        }

        private bool _EditMovesetsVisible;
        public bool EditMovesetsVisible
        {
            get
            {
                return this._EditMovesetsVisible;
            }
            set
            {
                Set(ref this._EditMovesetsVisible, value);
            }
        }
        private bool _OffenseDetailsVisible;
        public bool OffenseDetailsVisible
        {
            get
            {
                return this._OffenseDetailsVisible;
            }
            set
            {
                Set(ref this._OffenseDetailsVisible, value);
            }
        }
        private bool _DefenseDetailsVisible;
        public bool DefenseDetailsVisible
        {
            get
            {
                return this._DefenseDetailsVisible;
            }
            set
            {
                Set(ref this._DefenseDetailsVisible, value);
            }
        }
        private double _MinDPS;
        public double MinDPS
        {
            get
            {
                return this._MinDPS;
            }
            private set
            {
                Set(ref this._MinDPS, value);
            }
        }
        private double _MaxDPS;
        public double MaxDPS
        {
            get
            {
                return this._MaxDPS;
            }
            private set
            {
                Set(ref this._MaxDPS, value);
            }
        }
        #endregion

        #region Private Methods

        private void RemoveFastMove()
        {
            if (this.SelectedFastMove != null)
            {
                this.FastMoves.Remove(this.SelectedFastMove);
            }
        }

        private void RemoveChargeMove()
        {
            if (this.SelectedChargeMove != null)
            {
                this.ChargeMoves.Remove(this.SelectedChargeMove);
            }
        }

        private void ShowOffenseDetails()
        {
            this.EditMovesetsVisible = false;
            this.OffenseDetailsVisible = true;
            this.DefenseDetailsVisible = false;
            double minDps = double.MaxValue;
            double maxDps = double.MinValue;

            foreach (MovesetDetailsWrapper moveset in Movesets)
            {
                moveset.IsDefending = false;
                if (moveset.DPS >= maxDps)
                    maxDps = moveset.DPS;
                if (moveset.DPS <= minDps)
                    minDps = moveset.DPS;
            }
            foreach (MovesetDetailsWrapper moveset in Movesets)
                moveset.DPSUpperLimit = maxDps;
            this.MaxDPS = maxDps;
            this.MinDPS = minDps;
            this.MovesetsView.Refresh();
        }

        private void ShowDefenseDetails()
        {
            this.EditMovesetsVisible = false;
            this.OffenseDetailsVisible = false;
            this.DefenseDetailsVisible = true;
            double minDps = double.MaxValue;
            double maxDps = double.MinValue;

            foreach (MovesetDetailsWrapper moveset in Movesets)
            {
                moveset.IsDefending = true;
                if (moveset.DPS >= maxDps)
                    maxDps = moveset.DPS;
                if (moveset.DPS <= minDps)
                    minDps = moveset.DPS;
            }
            foreach (MovesetDetailsWrapper moveset in Movesets)
                moveset.DPSUpperLimit = maxDps;
            this.MaxDPS = maxDps;
            this.MinDPS = minDps;
            this.MovesetsView.Refresh();
        }

        private void ChargeMoves_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (PokedexChargeMoveWrapper chargeMove in e.NewItems)
                {
                    foreach (PokedexFastMoveWrapper fastMove in this.FastMoves)
                    {
                        this.Movesets.Add(new MovesetDetailsWrapper(new Moveset(fastMove as PokedexFastMoveWrapper, chargeMove as PokedexChargeMoveWrapper), this.Species.Type1, this.Species.Type2, this.Species.Attack, false));
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (PokedexChargeMoveWrapper chargeMove in e.OldItems)
                {
                    foreach (PokedexFastMoveWrapper fastMove in this.FastMoves)
                    {
                        this.Movesets.Remove(this.Movesets.FirstOrDefault(x => x.Moveset.FastMove == fastMove && x.Moveset.ChargeMove == chargeMove));
                    }
                }
            }
        }

        private void FastMoves_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (PokedexFastMoveWrapper fastMove in e.NewItems)
                {
                    foreach (PokedexChargeMoveWrapper chargeMove in this.ChargeMoves)
                    {
                        this.Movesets.Add(new MovesetDetailsWrapper(new Moveset(fastMove as PokedexFastMoveWrapper, chargeMove as PokedexChargeMoveWrapper), this.Species.Type1, this.Species.Type2, this.Species.Attack, false));
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (PokedexFastMoveWrapper fastMove in e.OldItems)
                {
                    foreach (PokedexChargeMoveWrapper chargeMove in this.ChargeMoves)
                    {
                        this.Movesets.Remove(this.Movesets.FirstOrDefault(x => x.Moveset.FastMove == fastMove && x.Moveset.ChargeMove == chargeMove));
                    }
                }
            }
        }

        private async void Exit()
        {
            if (this.savingNeeded)
            {
                MessageViewerEventArgs result = await this._messageViewer.DisplayMessage("Settings have not been saved, are you sure you want to exit?", "Discard Changes?", MessageViewerButton.OkCancel);
                if (result.Result == MessageViewerResult.Ok)
                    this.ClosePopup(null);
            }
            else
            {
                this.ClosePopup(null);
            }
        }

        private void Save()
        {
            //TODO: implement saving
            List<PokedexFastMoveWrapper> toRemoveFast = new List<PokedexFastMoveWrapper>();
            foreach (PokedexFastMoveWrapper fastMove in Species.FastMoves)
            {
                if (!this.FastMoves.Contains(fastMove))
                    toRemoveFast.Add(fastMove);
            }
            foreach (PokedexFastMoveWrapper fastMove in toRemoveFast)
                Species.FastMoves.Remove(fastMove);

            foreach (PokedexFastMoveWrapper fastMove in this.FastMoves)
            {
                if (!Species.FastMoves.Contains(fastMove))
                    Species.FastMoves.Add(fastMove);
            }

            List<PokedexChargeMoveWrapper> toRemoveCharge = new List<PokedexChargeMoveWrapper>();
            foreach (PokedexChargeMoveWrapper chargeMove in Species.ChargeMoves)
            {
                if (!this.ChargeMoves.Contains(chargeMove))
                    toRemoveCharge.Add(chargeMove);
            }
            foreach (PokedexChargeMoveWrapper chargeMove in toRemoveCharge)
                Species.ChargeMoves.Remove(chargeMove);

            foreach (PokedexChargeMoveWrapper chargeMove in this.ChargeMoves)
            {
                if (!Species.ChargeMoves.Contains(chargeMove))
                    Species.ChargeMoves.Add(chargeMove);
            }
            this.ClosePopup(null);
        }

        private void MoveCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.SetSavingNeeded();
        }

        private void SetSavingNeeded()
        {
            this.savingNeeded = true;
        }
        #endregion
    }
}
