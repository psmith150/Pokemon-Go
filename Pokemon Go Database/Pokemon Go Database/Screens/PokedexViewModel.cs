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
using System.Windows;

namespace Pokemon_Go_Database.Screens
{
    public class PokedexViewModel : ScreenViewModel
    {
        #region Commands
        public ICommand ShowMovesetsCommand { get; private set; }
        public ICommand JumpToTargetCommand { get; private set; }
        #endregion

        private readonly NavigationService navigationService;

        public PokedexViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.PokedexEntries = new ListCollectionView(this.Session.Pokedex);
            this.ShowMovesetsCommand = new RelayCommand<PokedexEntry>((species) => ShowMovesets(species));
            this.JumpToTargetCommand = new RelayCommand(() => this.JumpTo(this.JumpTargetName));
        }

        public override void Initialize()
        {

        }

        public override void Deinitialize()
        {

        }

        #region Public Properties
        private ListCollectionView _pokedexEntries;
        public ListCollectionView PokedexEntries
        {
            get
            {
                return this._pokedexEntries;
            }
            private set
            {
                this.Set(ref this._pokedexEntries, value);
                this._pokedexEntries.SortDescriptions.Add(new System.ComponentModel.SortDescription("Number", System.ComponentModel.ListSortDirection.Ascending));
            }
        }

        public string JumpTargetName;

        public Array Types
        {
            get
            {
                return Enum.GetValues(typeof(Model.Type));
            }
        }
        #endregion

        #region Private Methods
        private async void ShowMovesets(PokedexEntry species)
        {
            await this.navigationService.OpenPopup<EditMovesetsViewModel>(species);
        }

        private void JumpTo(string name)
        {
            PokedexEntry jumpTarget = this.Session.Pokedex.FirstOrDefault(x => x.Species.Equals(name));
            this.PokedexEntries.MoveCurrentTo(jumpTarget);
        }
        #endregion
    }
}
