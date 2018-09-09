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

        public IVCalculatorViewModel(SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this._messageViewer = messageViewer;
            this.ExitPopupCommand = new RelayCommand(() => Exit());
            this.SaveCommand = new RelayCommand(() => Save());
            this.CalculateIVsCommand = new RelayCommand(() => {
                this.Calculator.CalculateValues();
                if (this.Calculator.IVSets.Count > 0)
                {
                    this.SimulatedLevel = this.Calculator.IVSets.Min(x => x.Level);
                    this.MinLevel = this.Calculator.IVSets.Min(x => x.Level);
                }
            });

        }

        public override void Initialize(object param)
        {
            IVCalculatorWrapper wrapper = param as IVCalculatorWrapper;
            this.Calculator = wrapper.Calculator;
            this.IsNotNewPokemon = !wrapper.IsNewPokemon;
            this.savingNeeded = false;
            if (Calculator == null)
            {
                this._messageViewer.DisplayMessage("Invalid pokemon!", "Invalid Pokemon", MessageViewerButton.Ok);
            }
            this._originalFastMove = this.Calculator.Pokemon.FastMove;
            this._originalChargeMove = this.Calculator.Pokemon.ChargeMove;
            this.MinLevel = this.Calculator.Pokemon.Level;
            this.SimulatedLevel = this.Calculator.Pokemon.Level;
            this.Calculator.Pokemon.PropertyChanged += ((o, a) => { this.savingNeeded = true; if (a.PropertyName.Equals("IsLucky")){ this.RaisePropertyChanged("DustValues"); } });
            this.RaisePropertyChanged("DustValues");
        }

        public override void Deinitialize()
        {
            this.Calculator = null;
        }

        #region Private Fields
        private bool savingNeeded = false;
        private MessageViewerBase _messageViewer;
        private PokedexFastMoveWrapper _originalFastMove;
        private PokedexChargeMoveWrapper _originalChargeMove;
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
                if (this.Calculator.Pokemon.IsLucky)
                {
                    int[] dustCutoffs = new int[Constants.DustCutoffs.Length];
                    Array.Copy(Constants.DustCutoffs, dustCutoffs, Constants.DustCutoffs.Length);
                    for (int i = 0; i < dustCutoffs.Length; i++)
                        dustCutoffs[i] = (int)Math.Floor(dustCutoffs[i] * Constants.LuckyStardustMultiplier);
                    return dustCutoffs;
                }
                else
                {
                    return Constants.DustCutoffs;
                }
            }
        }
        private double _MinLevel = 1.0;
        public double MinLevel
        {
            get
            {
                return this._MinLevel;
            }
            private set
            {
                Set(ref this._MinLevel, value);
            }
        }
        private double _MaxLevel = Constants.MaxLevel;
        public double MaxLevel
        {
            get
            {
                return this._MaxLevel;
            }
            private set
            {
                Set(ref this._MaxLevel, value);
            }
        }
        private double _SimulatedLevel;
        public double SimulatedLevel
        {
            get
            {
                return this._SimulatedLevel;
            }
            set
            {
                Set(ref this._SimulatedLevel, value);
                this.SimulateLevelChange();
                this.RaiseSimulatedPropertiesChanged();
            }
        }

        private string _SimulatedAttack;
        public string SimulatedAttack
        {
            get
            {
                return this._SimulatedAttack;
            }
            private set
            {
                this.Set(ref this._SimulatedAttack, value);
            }
        }

        private string _SimulatedDefense;
        public string SimulatedDefense
        {
            get
            {
                return this._SimulatedDefense;
            }
            private set
            {
                this.Set(ref this._SimulatedDefense, value);
            }
        }

        private string _SimulatedStamina;
        public string SimulatedStamina
        {
            get
            {
                return this._SimulatedStamina;
            }
            private set
            {
                this.Set(ref this._SimulatedStamina, value);
            }
        }

        private string _SimulatedCP;
        public string SimulatedCP
        {
            get
            {
                return this._SimulatedCP;
            }
            private set
            {
                this.Set(ref this._SimulatedCP, value);
            }
        }

        private string _SimulatedDPS;
        public string SimulatedDPS
        {
            get
            {
                return this._SimulatedDPS;
            }
            private set
            {
                this.Set(ref this._SimulatedDPS, value);
            }
        }

        public int CandyRequired
        {
            get
            {
                return this.CalculateCandyRequired();
            }
        }

        public int StardustRequired
        {
            get
            {
                return this.CalculateStardustRequired();
            }
        }
        #endregion

        #region Private Methods

        private async void Exit()
        {
            if (this.savingNeeded)
            {
                MessageViewerEventArgs result = await this._messageViewer.DisplayMessage("Settings have not been saved, are you sure you want to exit?", "Discard Changes?", MessageViewerButton.OkCancel);
                if (result.Result == MessageViewerResult.Ok)
                {
                    this.Calculator.Pokemon.FastMove = this._originalFastMove;
                    this.Calculator.Pokemon.ChargeMove = this._originalChargeMove;
                    this.ClosePopup(null);
                }
            }
            else
            {
                this.Calculator.Pokemon.FastMove = this._originalFastMove;
                this.Calculator.Pokemon.ChargeMove = this._originalChargeMove;
                this.ClosePopup(null);
            }
        }

        private void Save()
        {
            List<int> attackIVs = new List<int>();
            List<int> defenseIVs = new List<int>();
            List<int> staminaIVs = new List<int>();
            List<double> levels = new List<double>();
            this.Calculator.Pokemon.IVSets.Clear();
            this.Calculator.Pokemon.IVSets.InsertRange(this.Calculator.IVSets);
            this.Calculator.Pokemon.GameCP = this.Calculator.Pokemon.ActualCP;
            this.Calculator.Pokemon.GameHP = this.Calculator.Pokemon.ActualHP;
            //foreach (IVSet combination in this.Calculator.IVSets)
            //{
            //    if (!attackIVs.Contains(combination.AttackIV))
            //        attackIVs.Add(combination.AttackIV);
            //    if (!defenseIVs.Contains(combination.DefenseIV))
            //        defenseIVs.Add(combination.DefenseIV);
            //    if (!staminaIVs.Contains(combination.StaminaIV))
            //        staminaIVs.Add(combination.StaminaIV);
            //    if (!levels.Contains(combination.Level))
            //        levels.Add(combination.Level);
            //}
            //this.Calculator.Pokemon.AttackIVExpression = string.Join("/", attackIVs.OrderBy(x => x).ToArray());
            //if (this.Calculator.Pokemon.AttackIVExpression.Equals(""))
            //    this.Calculator.Pokemon.AttackIVExpression = "0";
            //this.Calculator.Pokemon.DefenseIVExpression = string.Join("/", defenseIVs.OrderBy(x => x).ToArray());
            //if (this.Calculator.Pokemon.DefenseIVExpression.Equals(""))
            //    this.Calculator.Pokemon.DefenseIVExpression = "0";
            //this.Calculator.Pokemon.StaminaIVExpression = string.Join("/", staminaIVs.OrderBy(x => x).ToArray());
            //if (this.Calculator.Pokemon.StaminaIVExpression.Equals(""))
            //    this.Calculator.Pokemon.StaminaIVExpression = "0";
            //this.Calculator.Pokemon.LevelExpression = string.Join("/", levels.OrderBy(x => x).ToArray());
            //if (this.Calculator.Pokemon.LevelExpression.Equals(""))
            //    this.Calculator.Pokemon.LevelExpression = "1";

            if (!this.IsNotNewPokemon)
            {
                this.Calculator.Pokemon.Name = this.Calculator.GenerateName();
                this.ClosePopup(new IVCalculatorPopupEventArgs(this.Calculator.Pokemon));
            }
            else
                this.ClosePopup(null);
        }

        private int CalculateCandyRequired()
        {
            int candy = 0;
            int lookupIndex = 0;
            for (double i = this.MinLevel; i < this.SimulatedLevel; i+=0.5)
            {
                while (i >= Constants.DustLevelCutoffs[lookupIndex+1])
                    lookupIndex++;
                candy += Constants.CandyCutoffs[lookupIndex];
            }
            return candy;
        }
        private int CalculateStardustRequired()
        {
            int stardust = 0;
            int lookupIndex = 0;
            for (double i = this.MinLevel; i < this.SimulatedLevel; i+=0.5)
            {
                while (i >= Constants.DustLevelCutoffs[lookupIndex+1])
                    lookupIndex++;
                stardust += Constants.DustCutoffs[lookupIndex];
            }
            if (this.Calculator.Pokemon.IsLucky)
                stardust = (int)Math.Round(stardust * Constants.LuckyStardustMultiplier);
            return stardust;
        }

        private void SimulateLevelChange()
        {
            int min, max;
            if (this.Calculator.Pokemon.IVSets.Count <= 0)
                return;

            //Attack
            min = this.Calculator.Pokemon.IVSets.Min(x => x.AttackIV);
            max = this.Calculator.Pokemon.IVSets.Max(x => x.AttackIV);
            if (min != max)
            {
                this.SimulatedAttack = ((int)this.Calculator.Pokemon.GetAttack(min, this.SimulatedLevel)).ToString() + " - " + ((int)this.Calculator.Pokemon.GetAttack(max, this.SimulatedLevel)).ToString();
            }
            else
            {
                this.SimulatedAttack = ((int)this.Calculator.Pokemon.GetAttack(min, this.SimulatedLevel)).ToString();
            }

            //Defense
            min = this.Calculator.Pokemon.IVSets.Min(x => x.DefenseIV);
            max = this.Calculator.Pokemon.IVSets.Max(x => x.DefenseIV);
            if (min != max)
            {
                this.SimulatedDefense = ((int)this.Calculator.Pokemon.GetDefense(min, this.SimulatedLevel)).ToString() + " - " + ((int)this.Calculator.Pokemon.GetDefense(max, this.SimulatedLevel)).ToString();
            }
            else
            {
                this.SimulatedDefense = ((int)this.Calculator.Pokemon.GetDefense(min, this.SimulatedLevel)).ToString();
            }

            //Stamina
            min = this.Calculator.Pokemon.IVSets.Min(x => x.StaminaIV);
            max = this.Calculator.Pokemon.IVSets.Max(x => x.StaminaIV);
            if (min != max)
            {
                this.SimulatedStamina = ((int)this.Calculator.Pokemon.GetStamina(min, this.SimulatedLevel)).ToString() + " - " + ((int)this.Calculator.Pokemon.GetStamina(max, this.SimulatedLevel)).ToString();
            }
            else
            {
                this.SimulatedStamina = ((int)this.Calculator.Pokemon.GetStamina(min, this.SimulatedLevel)).ToString();
            }

            //CP
            List<int> possibleCP = new List<int>();
            foreach (IVSet combination in this.Calculator.Pokemon.IVSets)
            {
                possibleCP.Add(this.Calculator.Pokemon.GetCP(combination.AttackIV, combination.StaminaIV, combination.DefenseIV, this.SimulatedLevel));
            }
            min = possibleCP.Min(x => x);
            max = possibleCP.Max(x => x);
            if (min != max)
            {
                this.SimulatedCP = min.ToString() + " - " + max.ToString();
            }
            else
            {
                this.SimulatedCP = min.ToString();
            }

            //DPS
            List<double> possibleDPS = new List<double>();
            foreach (IVSet combination in this.Calculator.Pokemon.IVSets)
            {
                possibleDPS.Add(this.Calculator.Pokemon.Moveset.GetDPS(this.Calculator.Pokemon.GetAttack(combination.AttackIV, this.SimulatedLevel), this.Calculator.Pokemon.Species.Type1, this.Calculator.Pokemon.Species.Type2));
            }
            double minDPS = possibleDPS.Min(x => x);
            double maxDPS = possibleDPS.Max(x => x);
            if (Math.Abs(maxDPS - minDPS) > 0.001)
            {
                this.SimulatedDPS = minDPS.ToString("N2") + " - " + maxDPS.ToString("N2");
            }
            else
            {
                this.SimulatedDPS = minDPS.ToString("N2");
            }
        }

        private void RaiseSimulatedPropertiesChanged()
        {
            RaisePropertyChanged("CandyRequired");
            RaisePropertyChanged("StardustRequired");
        }
        #endregion
    }
}
