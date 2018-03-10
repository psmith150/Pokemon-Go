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
        public ICommand CalculateIVsCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        #endregion

        public IVCalculatorViewModel(SessionService session) : base(session)
        {

            this.ExitPopupCommand = new RelayCommand(() => Exit());
            this.SaveCommand = new RelayCommand(() => Save());
            this.CalculateIVsCommand = new RelayCommand(() => this.Calculator.CalculateValues());

        }

        public override void Initialize(object param)
        {
            IVCalculatorWrapper wrapper = param as IVCalculatorWrapper;
            this.Calculator = wrapper.Calculator;
            this.IsNotNewPokemon = !wrapper.IsNewPokemon;
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
            this.Calculator.Pokemon.AttackIVExpression = "";
            this.Calculator.Pokemon.DefenseIVExpression = "";
            this.Calculator.Pokemon.StaminaIVExpression = "";
            this.Calculator.Pokemon.LevelExpression = "";
            List<int> attackIVs = new List<int>();
            List<int> defenseIVs = new List<int>();
            List<int> staminaIVs = new List<int>();
            List<double> levels = new List<double>();
            foreach (ValueCombination combination in this.Calculator.ValueCombinations)
            {
                if (!attackIVs.Contains(combination.AttackIV))
                    attackIVs.Add(combination.AttackIV);
                if (!defenseIVs.Contains(combination.DefenseIV))
                    defenseIVs.Add(combination.DefenseIV);
                if (!staminaIVs.Contains(combination.StaminaIV))
                    staminaIVs.Add(combination.StaminaIV);
                if (!levels.Contains(combination.Level))
                    levels.Add(combination.Level);
            }
            this.Calculator.Pokemon.AttackIVExpression = string.Join("/", attackIVs.OrderBy(x => x).ToArray());
            if (this.Calculator.Pokemon.AttackIVExpression.Equals(""))
                this.Calculator.Pokemon.AttackIVExpression = "0";
            this.Calculator.Pokemon.DefenseIVExpression = string.Join("/", defenseIVs.OrderBy(x => x).ToArray());
            if (this.Calculator.Pokemon.DefenseIVExpression.Equals(""))
                this.Calculator.Pokemon.DefenseIVExpression = "0";
            this.Calculator.Pokemon.StaminaIVExpression = string.Join("/", staminaIVs.OrderBy(x => x).ToArray());
            if (this.Calculator.Pokemon.StaminaIVExpression.Equals(""))
                this.Calculator.Pokemon.StaminaIVExpression = "0";
            this.Calculator.Pokemon.LevelExpression = string.Join("/", levels.OrderBy(x => x).ToArray());
            if (this.Calculator.Pokemon.LevelExpression.Equals(""))
                this.Calculator.Pokemon.LevelExpression = "1";

            if (!this.IsNotNewPokemon)
                this.ClosePopup(new IVCalculatorPopupEventArgs(this.Calculator.Pokemon));
            else
                this.ClosePopup(null);
        }
        #endregion
    }
}
