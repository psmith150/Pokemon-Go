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
        public ICommand AddFilterCommand { get; private set; }
        public ICommand RemoveFilterCommand { get; private set; }
        #endregion

        private readonly NavigationService navigationService;

        public PokedexViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.PokedexEntries = new ListCollectionView(this.Session.Pokedex);
            this.ShowMovesetsCommand = new RelayCommand<PokedexEntry>((species) => ShowMovesets(species));
            this.JumpToTargetCommand = new RelayCommand(() => this.JumpTo(this.JumpTargetName));
            this.AddFilterCommand = new RelayCommand(() => this.AddFilter());
            this.RemoveFilterCommand = new RelayCommand<PokedexFilterElement>((filter) => this.RemoveFilter(filter));

            this.Filters = new MyObservableCollection<PokedexFilterElement>();
            this.Filters.CollectionChanged += (o, a) => this.UpdateFilter();
            this.Filters.MemberChanged += (o, a) => this.UpdateFilter();
        }

        public override void Initialize()
        {
            PokedexChargeMoveWrapper value1 = new PokedexChargeMoveWrapper(new ChargeMove("Test move 1"));
            PokedexChargeMoveWrapper value2 = new PokedexChargeMoveWrapper(new ChargeMove("Test move 2"));
            this.TestValues = new List<PokedexChargeMoveWrapper>(new PokedexChargeMoveWrapper[] { value1, value2 });
            this.TestValue = value1;
        }

        public override void Deinitialize()
        {

        }

        #region Public Properties
        private List<PokedexChargeMoveWrapper> _testValues;
        public List<PokedexChargeMoveWrapper> TestValues
        {
            get
            {
                return this._testValues;
            }
            set
            {
                this.Set(ref this._testValues, value);
            }
        }
        private PokedexChargeMoveWrapper _testValue;
        public PokedexChargeMoveWrapper TestValue
        {
            get
            {
                return this._testValue;
            }
            set
            {
                this.Set(ref this._testValue, value);
            }
        }
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
                this.PokedexEntries.SortDescriptions.Add(new System.ComponentModel.SortDescription("Number", System.ComponentModel.ListSortDirection.Ascending));
                this.PokedexEntries.Filter = this.PokedexFilter;
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
        private MyObservableCollection<PokedexFilterElement> _Filters;
        public MyObservableCollection<PokedexFilterElement> Filters
        {
            get
            {
                return this._Filters;
            }
            private set
            {
                this.Set(ref this._Filters, value);
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
        private bool PokedexFilter(object item)
        {
            PokedexEntry pokemon = item as PokedexEntry;
            bool result = true;
            foreach (PokedexFilterElement filter in this.Filters)
            {
                if (!filter.EvaluateFilter(pokemon))
                    result = false;
            }
            return result;
        }
        private void UpdateFilter()
        {
            this.PokedexEntries.Refresh();
        }
        private void AddFilter()
        {
            this.Filters.Add(new PokedexFilterElement());
        }
        private void RemoveFilter(PokedexFilterElement filter)
        {
            if (this.Filters != null && this.Filters.Contains(filter))
                this.Filters.Remove(filter);
        }
        #endregion
    }
}
