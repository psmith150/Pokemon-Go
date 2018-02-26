using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
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
        }

        #region Public Properties
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
        #endregion

        #region Public Methods
        public void CalculateValues()
        {
            if (this.Pokemon == null)
                return;
            List<double> possibleLevels = new List<double>();
            List<int> possibleAttackIVs = new List<int>();
            List<int> possibleDefenseIVs = new List<int>();
            List<int> possibleStaminaIVs = new List<int>();
            List<ValueCombination> combinations = new List<ValueCombination>();
            foreach (double level in possibleLevels)
            {
                foreach (int attackIV in possibleAttackIVs)
                {
                    foreach (int staminaIV in possibleStaminaIVs)
                    {
                        foreach (int defenseIV in possibleDefenseIVs)
                        {
                            int cp = Pokemon.GetCP(attackIV, staminaIV, defenseIV, level);
                            int hp = Pokemon.GetStamina(staminaIV, level);
                            if (cp == Pokemon.ActualCP && hp == Pokemon.ActualHP)
                            {
                                combinations.Add(new ValueCombination(attackIV, staminaIV, defenseIV, level));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Value Combination Class
        private class ValueCombination
        {
            public ValueCombination(int attackIV = 0, int staminaIV = 0, int defenseIV = 0, double level = 1.0)
            {
                AttackIV = attackIV;
                StaminaIV = staminaIV;
                DefenseIV = defenseIV;
                Level = level;
            }
            public int AttackIV { get; set; }
            public int StaminaIV { get; set; }
            public int DefenseIV { get; set; }
            public double Level { get; set; }
        }
        #endregion
    }
}
