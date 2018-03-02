using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class ValueCombination
    {
        public ValueCombination(int attackIV = 0, int defenseIV = 0, int staminaIV = 0, double level = 1.0)
        {
            AttackIV = attackIV;
            DefenseIV = defenseIV;
            StaminaIV = staminaIV;
            Level = level;
            IVPercentage = (AttackIV + DefenseIV + StaminaIV) / (3.0 * Constants.MaxIV);
        }
        public int AttackIV { get; set; }
        public int DefenseIV { get; set; }
        public int StaminaIV { get; set; }
        public double Level { get; set; }
        public double IVPercentage { get; set; }
    }
}
