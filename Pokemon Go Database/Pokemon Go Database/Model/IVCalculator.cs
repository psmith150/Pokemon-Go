using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class IVCalculator : ObservableObject
    {
        public IVCalculator(Pokemon pokemon)
        {
            this.Pokemon = pokemon;
            if (this.Pokemon.GetAttackIV() >= this.Pokemon.GetDefenseIV() && this.Pokemon.GetAttackIV() >= this.Pokemon.GetStaminaIV())
            {
                this.AttackBest = true;
            }
            if (this.Pokemon.GetDefenseIV() >= this.Pokemon.GetAttackIV() && this.Pokemon.GetDefenseIV() >= this.Pokemon.GetStaminaIV())
            {
                this.DefenseBest = true;
            }
            if (this.Pokemon.GetStaminaIV() >= this.Pokemon.GetAttackIV() && this.Pokemon.GetStaminaIV() >= this.Pokemon.GetDefenseIV())
            {
                this.StaminaBest = true;
            }
            this.GetIVLevels();
            this.ValueCombinations = new ObservableCollection<ValueCombination>();
        }

        #region Public Properties
        private ObservableCollection<ValueCombination> _ValueCombinations;
        public ObservableCollection<ValueCombination> ValueCombinations
        {
            get
            {
                return this._ValueCombinations;
            }
            set
            {
                Set(ref this._ValueCombinations, value);
            }
        }
        private Pokemon _Pokemon;
        public Pokemon Pokemon
        {
            get
            {
                return _Pokemon;
            }
            private set
            {
                Set(ref this._Pokemon, value);
            }
        }
        private bool _AttackBest;
        public bool AttackBest
        {
            get
            {
                return _AttackBest;
            }
            set
            {
                Set(ref this._AttackBest, value);
            }
        }
        private bool _DefenseBest;
        public bool DefenseBest
        {
            get
            {
                return _DefenseBest;
            }
            set
            {
                Set(ref this._DefenseBest, value);
            }
        }
        private bool _StaminaBest;
        public bool StaminaBest
        {
            get
            {
                return _StaminaBest;
            }
            set
            {
                Set(ref this._StaminaBest, value);
            }
        }
        public IVLevel BestIVLevel { get; set; }
        public TotalIVLevel TotalIVLevel { get; set; }
        #endregion

        #region Private Fields
        private IVLevel _AttackIVLevel;
        private IVLevel _DefenseIVLevel;
        private IVLevel _StaminaIVLevel;
        #endregion

        #region Public Methods
        public void CalculateValues()
        {
            if (this.Pokemon == null)
                return;
            List<double> possibleLevels = GetPossibleLevels();
            List<int> possibleAttackIVs = GetPossibleIVs(this._AttackIVLevel);
            List<int> possibleDefenseIVs = GetPossibleIVs(this._DefenseIVLevel);
            List<int> possibleStaminaIVs = GetPossibleIVs(this._StaminaIVLevel);
            this.ValueCombinations.Clear();
            foreach (double level in possibleLevels)
            {
                foreach (int attackIV in possibleAttackIVs)
                {
                    foreach (int defenseIV in possibleDefenseIVs)
                    {
                        foreach (int staminaIV in possibleStaminaIVs)
                        {
                            int cp = Pokemon.GetCP(attackIV, staminaIV, defenseIV, level);
                            int hp = Pokemon.GetStamina(staminaIV, level);
                            if (cp == Pokemon.ActualCP && hp == Pokemon.ActualHP)
                            {
                                this.ValueCombinations.Add(new ValueCombination(attackIV, defenseIV, staminaIV, level));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void GetIVLevels()
        {
            TotalIVLevel totalIVLevel = TotalIVLevel.Low;
            for (int i=0; i<Constants.IVLevelCutoffs.Count(); i++)
            {
                if (this.Pokemon.GetAttackIV() >= Constants.IVLevelCutoffs[i])
                    _AttackIVLevel = (IVLevel)i;
                if (this.Pokemon.GetDefenseIV() >= Constants.IVLevelCutoffs[i])
                    _DefenseIVLevel = (IVLevel)i;
                if (this.Pokemon.GetStaminaIV() >= Constants.IVLevelCutoffs[i])
                    _StaminaIVLevel = (IVLevel)i;
                if (Pokemon.GetAttackIV() + Pokemon.GetStaminaIV() + Pokemon.GetDefenseIV() >= Constants.IVSumCutoffs[i])
                    totalIVLevel = (TotalIVLevel)i;
            }
            IVLevel[] tempArray = {_AttackIVLevel, _DefenseIVLevel, _StaminaIVLevel};
            this.BestIVLevel = tempArray.Max();
            this.TotalIVLevel = totalIVLevel;
        }

        private List<int> GetPossibleIVs(IVLevel level)
        {
            List<int> values = new List<int>();
            int endIndex = (level == IVLevel.Max) ? Constants.IVLevelCutoffs.Max()+1 : Constants.IVLevelCutoffs[(int)level + 1];
            int startIndex = Constants.IVLevelCutoffs[(int)level];
            for (int i = startIndex; i < endIndex; i++)
            {
                values.Add(i);
            }
            return values;
        }
        private List<double> GetPossibleLevels()
        {
            List<double> values = new List<double>();
            double endIndex = (this.Pokemon.DustToPower == Constants.DustCutoffs.Max()) ? Constants.DustLevelCutoffs.Max() + 0.5 : Constants.DustLevelCutoffs[Array.IndexOf(Constants.DustCutoffs, this.Pokemon.DustToPower)+1];
            double startIndex = Constants.DustLevelCutoffs[Array.IndexOf(Constants.DustCutoffs, this.Pokemon.DustToPower)];
            for (double i = startIndex; i < endIndex; i+= 0.5)
            {
                if (!this.Pokemon.HasBeenPowered && (int)(i * 2) % 2 == 1)
                    continue;
                values.Add(i);
            }
            return values;
        }
        #endregion
    }
}
