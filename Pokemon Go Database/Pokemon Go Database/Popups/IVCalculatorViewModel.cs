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
    public class IVCalculatorViewModel : PopupViewModel
    {
        #region Commands
        public ICommand ExitPopupCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        #endregion

        public IVCalculatorViewModel(SessionService session) : base(session)
        {

            this.ExitPopupCommand = new RelayCommand(() => Exit());
            this.SaveCommand = new RelayCommand(() => Save());

        }

        public override void Initialize(object param)
        {
            this.Calculator = param as IVCalculator;
            if (Calculator == null)
            {
                MessageBox.Show("Invalid pokemon!", "Invalid Pokemon", MessageBoxButton.OK);
            }
        }

        public override void Deinitialize()
        {
            this.Calculator = null;
        }

        #region Private Fields
        private bool savingNeeded = false;
        #endregion

        #region Public Properties
        private IVCalculator _Calculator;
        public IVCalculator Calculator
        {
            get
            {
                return this._Calculator;
            }
            set
            {
                Set(ref this._Calculator, value);
            }
        }
        public bool IsNotNewPokemon { get; set; }
        public Array IVLevels
        {
            get
            {
                return Enum.GetValues(typeof(Model.IVLevel));
            }
        }
        public Array TotalIVLevels
        {
            get
            {
                return Enum.GetValues(typeof(Model.TotalIVLevel));
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
                this.ClosePopup(null);
            }
        }

        private void Save()
        {
            //TODO: implement saving
            this.ClosePopup(null);
        }
        #endregion
    }
}
