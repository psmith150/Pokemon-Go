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

namespace Pokemon_Go_Database.Screens
{
    public class PokedexViewModel : ScreenViewModel
    {
        #region Commands
        public ICommand ShowMovesetsCommand { get; private set; }
        #endregion

        private readonly NavigationService navigationService;

        public PokedexViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.PokedexEntries = this.Session.Pokedex;
            this.ShowMovesetsCommand = new RelayCommand<PokedexEntry>((species) => ShowMovesets(species));
        }

        public override void Initialize()
        {

        }

        public override void Deinitialize()
        {

        }

        #region Public Properties
        private MyObservableCollection<PokedexEntry> _pokedexEntries;
        public MyObservableCollection<PokedexEntry> PokedexEntries
        {
            get
            {
                return this._pokedexEntries;
            }
            private set
            {
                this.Set(ref this._pokedexEntries, value);
            }
        }
        #endregion

        #region Private Methods
        private async void ShowMovesets(PokedexEntry species)
        {
            await this.navigationService.OpenPopup<EditMovesetsViewModel>(species);
        }
        #endregion
    }
}
