using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class IVCalculatorWrapper
    {
        public IVCalculatorWrapper(IVCalculator calculator, bool isNewPokemon = false)
        {
            this.Calculator = calculator;
            this.IsNewPokemon = isNewPokemon;
        }
        public IVCalculator Calculator { get; set; }
        public bool IsNewPokemon { get; set; }
    }
}
