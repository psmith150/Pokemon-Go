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
            this.MyPokemon = this.Session.MyPokemon;
        }

        public override void Initialize()
        {

        }

        public override void Deinitialize()
        {

        }

        #region Public Properties
        public Pokemon SelectedPokemon { get; set; }
        private MyObservableCollection<Pokemon> _MyPokemon;
        public MyObservableCollection<Pokemon> MyPokemon
        {
            get
            {
                return this._MyPokemon;
            }
            private set
            {
                this.Set(ref this._MyPokemon, value);
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
        #endregion

        #region Private Methods
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
