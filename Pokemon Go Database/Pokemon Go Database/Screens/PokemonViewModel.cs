using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_Go_Database.Services;
using System.Windows.Data;
using Pokemon_Go_Database.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using Pokemon_Go_Database.Popups;
using System.Collections.ObjectModel;

namespace Pokemon_Go_Database.Screens
{
    public class PokemonViewModel : ScreenViewModel
    {
        #region Commands
        public ICommand CheckIVCommand { get; private set; }
        public ICommand AddNewPokemonCommand { get; private set; }
        public ICommand ShowMovesetsCommand { get; private set; }
        public ICommand GoToSpeciesCommand { get; private set; }
        #endregion

        private readonly NavigationService navigationService;

        public PokemonViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.CheckIVCommand = new RelayCommand(async () => await CheckIVAsync());
            this.AddNewPokemonCommand = new RelayCommand(async () => await AddNewPokemonAsync());
            this.ShowMovesetsCommand = new RelayCommand(async () => await ShowMovesetsAsync());
            this.GoToSpeciesCommand = new RelayCommand(() => GoToSpecies());

            this.AllSpecies = this.Session.Pokedex;
            this.MyPokemon = new ListCollectionView(this.Session.MyPokemon);
        }

        public override void Initialize()
        {

        }

        public override void Deinitialize()
        {

        }

        #region Public Properties
        public Pokemon SelectedPokemon { get; set; }
        private ListCollectionView _MyPokemon;
        public ListCollectionView MyPokemon
        {
            get
            {
                return this._MyPokemon;
            }
            private set
            {
                this.Set(ref this._MyPokemon, value);
                this._MyPokemon.SortDescriptions.Add(new System.ComponentModel.SortDescription("ActualCP", System.ComponentModel.ListSortDirection.Descending));
                this._MyPokemon.Filter = PokemonFilter;
            }
        }

        private MyObservableCollection<PokedexEntry> _AllSpecies;
        public MyObservableCollection<PokedexEntry> AllSpecies
        {
            get
            {
                return _AllSpecies;
            }
            private set
            {
                Set(ref this._AllSpecies, value);
            }
        }
        public int[] DustValues
        {
            get
            {
                return Constants.DustCutoffs;
            }
        }
        private bool _ComparisonFilterActive;
        public bool ComparisonFilterActive
        {
            get
            {
                return this._ComparisonFilterActive;
            }
            set
            {
                Set(ref this._ComparisonFilterActive, value);
                UpdateFilter();
            }
        }
        private bool _FavoriteFilterActive;
        public bool FavoriteFilterActive
        {
            get
            {
                return this._FavoriteFilterActive;
            }
            set
            {
                Set(ref this._FavoriteFilterActive, value);
                UpdateFilter();
            }
        }
        private bool _FastTMFilterActive;
        public bool FastTMFilterActive
        {
            get
            {
                return this._FastTMFilterActive;
            }
            set
            {
                Set(ref this._FastTMFilterActive, value);
                UpdateFilter();
            }
        }
        private bool _ChargeTMFilterActive;
        public bool ChargeTMFilterActive
        {
            get
            {
                return this._ChargeTMFilterActive;
            }
            set
            {
                Set(ref this._ChargeTMFilterActive, value);
                UpdateFilter();
            }
        }
        private bool _PowerUpFilterActive;
        public bool PowerUpFilterActive
        {
            get
            {
                return this._PowerUpFilterActive;
            }
            set
            {
                Set(ref this._PowerUpFilterActive, value);
                UpdateFilter();
            }
        }
        #endregion

        #region Private Methods
        private bool PokemonFilter(object item)
        {
            Pokemon pokemon = item as Pokemon;
            bool result = true;
            if (this.ComparisonFilterActive && !pokemon.Compare)
                result = false;
            if (this.FavoriteFilterActive && !pokemon.IsFavorite)
                result = false;
            if (this.FastTMFilterActive && !pokemon.NeedsFastTM)
                result = false;
            if (this.ChargeTMFilterActive && !pokemon.NeedsChargeTM)
                result = false;
            if (this.PowerUpFilterActive && !pokemon.ShouldBePoweredUp)
                result = false;
            return result;
        }
        private void UpdateFilter()
        {
            this.MyPokemon.Refresh();
        }
        private async Task CheckIVAsync()
        {
            if (this.SelectedPokemon == null)
                return;
            await navigationService.OpenPopup<IVCalculatorViewModel>(new IVCalculatorWrapper(new IVCalculator(this.SelectedPokemon)));
        }
        private async Task AddNewPokemonAsync()
        {
            Pokemon newPokemon = new Pokemon();
            IVCalculator calculator = new IVCalculator(newPokemon)
            {
                AttackBest = false,
                DefenseBest = false,
                StaminaBest = false
            };
            IVCalculatorPopupEventArgs args =  await navigationService.OpenPopup<IVCalculatorViewModel>(new IVCalculatorWrapper(calculator, true)) as IVCalculatorPopupEventArgs;
            if (args != null)
                this.Session.MyPokemon.Add(args.NewPokemon);
        }
        private async Task ShowMovesetsAsync()
        {
            if (this.SelectedPokemon == null)
                return;
            await this.navigationService.OpenPopup<EditMovesetsViewModel>(this.SelectedPokemon.Species);
        }
        private void GoToSpecies()
        {
            this.navigationService.NavigateTo<PokedexViewModel>();
        }
        #endregion
    }
}
