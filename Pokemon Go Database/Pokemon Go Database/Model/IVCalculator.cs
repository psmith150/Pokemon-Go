using GalaSoft.MvvmLight;
using Pokemon_Go_Database.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            this.InitIVLevels(true);
            this.IVSets = new ObservableCollection<IVSet>();
        }

        #region Public Properties
        private ObservableCollection<IVSet> _IVSets;
        public ObservableCollection<IVSet> IVSets
        {
            get
            {
                return this._IVSets;
            }
            set
            {
                Set(ref this._IVSets, value);
            }
        }
        public double AverageIVPercentage
        {
            get
            {
                if (this.IVSets.Count <= 0)
                    return 0.0;
                return this.IVSets.Select(x => x.IVPercentage).Average();
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
        private IVLevel attackIVLevel;
        private IVLevel defenseIVLevel;
        private IVLevel staminaIVLevel;
        #endregion

        #region Public Methods
        public void CalculateValues()
        {
            if (this.Pokemon == null)
                return;
            this.UpdateIVLevels();
            List<double> possibleLevels = GetPossibleLevels();
            List<int> possibleAttackIVs = GetPossibleIVs(this.attackIVLevel, this.AttackBest);
            List<int> possibleDefenseIVs = GetPossibleIVs(this.defenseIVLevel, this.DefenseBest);
            List<int> possibleStaminaIVs = GetPossibleIVs(this.staminaIVLevel, this.StaminaBest);
            this.IVSets.Clear();
            foreach (double level in possibleLevels)
            {
                foreach (int attackIV in possibleAttackIVs)
                {
                    foreach (int defenseIV in possibleDefenseIVs)
                    {
                        if (this.DefenseBest && this.AttackBest && defenseIV != attackIV)
                            continue;
                        foreach (int staminaIV in possibleStaminaIVs)
                        {
                            if (!this.ValidateIVs(attackIV, defenseIV, staminaIV))
                                continue;
                            int cp = Pokemon.GetCP(attackIV, staminaIV, defenseIV, level);
                            int hp = (int)Pokemon.GetStamina(staminaIV, level);
                            if (cp == Pokemon.GameCP && hp == Pokemon.GameHP)
                            {
                                this.IVSets.Add(new IVSet(attackIV, defenseIV, staminaIV, level));
                            }
                        }
                    }
                }
            }
            RaisePropertyChanged("AverageIVPercentage");
        }

        public string GenerateName()
        {
            string templateString = Settings.Default.SuggestedName;
            Regex templateRegex = new Regex(@"<(.*?)>", RegexOptions.Compiled);
            var matches = templateRegex.Matches(templateString);

            if (matches?.Count > 0)
            {
                foreach (var match in matches.Cast<Match>())
                {
                    string replacementValue = "";
                    switch (match.Groups[1].Value)
                    {
                        case "Species":
                            replacementValue = this.Pokemon.Species.Species;
                            break;
                        case "IV%":
                            replacementValue = string.Format("{0:0}", this.AverageIVPercentage * 100.0);
                            break;
                        default:
                            break;
                    }
                    templateString = templateString.Replace(match.Value, replacementValue);
                }
            }
            return templateString;
        }
        #endregion

        #region Private Methods
        private void InitIVLevels(bool evaluateTotalLevel = false)
        {
            TotalIVLevel totalIVLevel = TotalIVLevel.Low;
            for (int i = 0; i < Constants.IVLevelCutoffs.Count(); i++)
            {
                if (this.AttackBest && this.Pokemon.GetAttackIV() >= Constants.IVLevelCutoffs[i])
                    attackIVLevel = (IVLevel)i;
                if (this.DefenseBest && this.Pokemon.GetDefenseIV() >= Constants.IVLevelCutoffs[i])
                    defenseIVLevel = (IVLevel)i;
                if (this.StaminaBest && this.Pokemon.GetStaminaIV() >= Constants.IVLevelCutoffs[i])
                    staminaIVLevel = (IVLevel)i;
                if (Pokemon.GetAttackIV() + Pokemon.GetStaminaIV() + Pokemon.GetDefenseIV() >= Constants.IVSumCutoffs[i])
                    totalIVLevel = (TotalIVLevel)i;
            }
            IVLevel[] tempArray = { attackIVLevel, defenseIVLevel, staminaIVLevel };
            this.BestIVLevel = tempArray.Max();
            if (evaluateTotalLevel)
                this.TotalIVLevel = totalIVLevel;
        }
        private void UpdateIVLevels(bool evaluateTotalLevel = false)
        {
            attackIVLevel = IVLevel.Low;
            defenseIVLevel = IVLevel.Low;
            staminaIVLevel = IVLevel.Low;
            if (this.AttackBest)
                attackIVLevel = this.BestIVLevel;
            if (this.DefenseBest)
                defenseIVLevel = this.BestIVLevel;
            if (this.StaminaBest)
                staminaIVLevel = this.BestIVLevel;
        }

        private List<int> GetPossibleIVs(IVLevel level, bool isBest)
        {
            List<int> values = new List<int>();
            int endIndex = (level == IVLevel.Max) ? Constants.MaxIV + 1 : Constants.IVLevelCutoffs[(int)level + 1];
            int startIndex = Constants.IVLevelCutoffs[(int)level];
            if (!isBest)
            {
                endIndex = Constants.MaxIV;
                startIndex = 0;
            }
            for (int i = startIndex; i < endIndex; i++)
            {
                values.Add(i);
            }
            return values;
        }
        private List<int> GetPossibleIVSums(TotalIVLevel level)
        {
            List<int> values = new List<int>();
            int endIndex = (level == TotalIVLevel.Max) ? 3 * Constants.MaxIV + 1 : Constants.IVSumCutoffs[(int)level + 1];
            int startIndex = Constants.IVSumCutoffs[(int)level];
            for (int i = startIndex; i < endIndex; i++)
            {
                values.Add(i);
            }
            return values;
        }
        private List<double> GetPossibleLevels()
        {
            List<double> values = new List<double>();
            double endIndex = (this.Pokemon.DustToPower == Constants.DustCutoffs.Max()) ? Constants.DustLevelCutoffs.Max() + 0.5 : Constants.DustLevelCutoffs[Array.IndexOf(Constants.DustCutoffs, this.Pokemon.DustToPower) + 1];
            double startIndex = Constants.DustLevelCutoffs[Array.IndexOf(Constants.DustCutoffs, this.Pokemon.DustToPower)];
            for (double i = startIndex; i < endIndex; i += 0.5)
            {
                if (!this.Pokemon.HasBeenPowered && (int)(i * 2) % 2 == 1)
                    continue;
                values.Add(i);
            }
            return values;
        }
        private bool ValidateIVs(int attackIV, int defenseIV, int staminaIV)
        {
            if (!this.AttackBest && !this.DefenseBest && this.StaminaBest)
            {
                if (staminaIV <= attackIV || staminaIV <= defenseIV)
                    return false;
            }
            else if (!this.AttackBest && this.DefenseBest && !this.StaminaBest)
            {
                if (defenseIV <= attackIV || defenseIV <= staminaIV)
                    return false;
            }
            else if (!this.AttackBest && this.DefenseBest && this.StaminaBest)
            {
                if (staminaIV <= attackIV || staminaIV != defenseIV)
                    return false;
            }
            else if (this.AttackBest && !this.DefenseBest && !this.StaminaBest)
            {
                if (attackIV <= defenseIV || attackIV <= staminaIV)
                    return false;
            }
            else if (this.AttackBest && !this.DefenseBest && this.StaminaBest)
            {
                if (attackIV <= defenseIV || attackIV != staminaIV)
                    return false;
            }
            else if (this.AttackBest && this.DefenseBest && !this.StaminaBest)
            {
                if (attackIV != defenseIV || attackIV <= staminaIV)
                    return false;
            }
            else if (this.AttackBest && this.DefenseBest && this.StaminaBest)
            {
                if (attackIV != defenseIV || attackIV != staminaIV)
                    return false;
            }
            else
            {
                return false;
            }
            return this.GetPossibleIVSums(this.TotalIVLevel).Contains((attackIV + staminaIV + defenseIV)); // Final check
        }
        #endregion
    }
}
