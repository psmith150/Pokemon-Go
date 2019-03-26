using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class PokemonFilterElement : ObservableObject
    {
        #region Constructor
        public PokemonFilterElement()
        {
            this.FilterType = PokemonFilterType.Name;
            this.ComparisonType = FilterComparisonType.Equal;
            this.FilterValue = "";
        }
        #endregion
        #region Public Properties
        private PokemonFilterType _FilterType;
        public PokemonFilterType FilterType
        {
            get
            {
                return this._FilterType;
            }
            set
            {
                this.Set(ref this._FilterType, value);
                this.FilterValue = "";
            }
        }
        private FilterComparisonType _ComparisonType;
        public FilterComparisonType ComparisonType
        {
            get
            {
                return this._ComparisonType;
            }
            set
            {
                this.Set(ref this._ComparisonType, value);
            }
        }
        private string _FilterValue;
        public string FilterValue
        {
            get
            {
                return this._FilterValue;
            }
            set
            {
                this.Set(ref this._FilterValue, value);
            }
        }
        #endregion
        #region Public Methods
        public bool EvaluateFilter(Pokemon pokemon)
        {
            bool test = false;
            switch (this.FilterType)
            {
                case PokemonFilterType.Name:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Name.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.Name, this.FilterValue, true) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.Name, this.FilterValue, true) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.Name, this.FilterValue, true) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.Name, this.FilterValue, true) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !pokemon.Name.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        default:
                            return false;
                    }
                case PokemonFilterType.Species:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Species.Species.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.Species.Species, this.FilterValue, true) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.Species.Species, this.FilterValue, true) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.Species.Species, this.FilterValue, true) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.Species.Species, this.FilterValue, true) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !pokemon.Species.Species.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        default:
                            return false;
                    }
                case PokemonFilterType.Type:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Species.Type1.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.Species.Type2.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.Species.Type1.ToString(), this.FilterValue, true) >= 1 || string.Compare(pokemon.Species.Type2.ToString(), this.FilterValue, true) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.Species.Type1.ToString(), this.FilterValue, true) <= -1 || string.Compare(pokemon.Species.Type2.ToString(), this.FilterValue, true) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.Species.Type1.ToString(), this.FilterValue, true) >= 0 || string.Compare(pokemon.Species.Type2.ToString(), this.FilterValue, true) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.Species.Type1.ToString(), this.FilterValue, true) <= 0 || string.Compare(pokemon.Species.Type2.ToString(), this.FilterValue, true) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !(pokemon.Species.Type1.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.Species.Type2.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase));
                        default:
                            return false;
                    }
                case PokemonFilterType.MoveType:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.FastMove.FastMove.Type.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.ChargeMove.ChargeMove.Type.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.FastMove.FastMove.Type.ToString(), this.FilterValue, true) >= 1 || string.Compare(pokemon.ChargeMove.ChargeMove.Type.ToString(), this.FilterValue, true) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.FastMove.FastMove.Type.ToString(), this.FilterValue, true) <= -1 || string.Compare(pokemon.ChargeMove.ChargeMove.Type.ToString(), this.FilterValue, true) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.FastMove.FastMove.Type.ToString(), this.FilterValue, true) >= 0 || string.Compare(pokemon.ChargeMove.ChargeMove.Type.ToString(), this.FilterValue, true) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.FastMove.FastMove.Type.ToString(), this.FilterValue, true) <= 0 || string.Compare(pokemon.ChargeMove.ChargeMove.Type.ToString(), this.FilterValue, true) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !(pokemon.FastMove.FastMove.Type.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.ChargeMove.ChargeMove.Type.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase));
                        default:
                            return false;
                    }
                case PokemonFilterType.Move:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.FastMove.FastMove.Name.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.ChargeMove.ChargeMove.Name.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.FastMove.FastMove.Name, this.FilterValue, true) >= 1 || string.Compare(pokemon.ChargeMove.ChargeMove.Name, this.FilterValue, true) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.FastMove.FastMove.Name, this.FilterValue, true) <= -1 || string.Compare(pokemon.ChargeMove.ChargeMove.Name, this.FilterValue, true) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.FastMove.FastMove.Name, this.FilterValue, true) >= 0 || string.Compare(pokemon.ChargeMove.ChargeMove.Name, this.FilterValue, true) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.FastMove.FastMove.Name, this.FilterValue, true) <= 0 || string.Compare(pokemon.ChargeMove.ChargeMove.Name, this.FilterValue, true) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !(pokemon.FastMove.FastMove.Name.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.ChargeMove.ChargeMove.Name.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase));
                        default:
                            return false;
                    }
                case PokemonFilterType.CP:
                    int cp;
                    test = int.TryParse(this.FilterValue, out cp);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.ActualCP == cp;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.ActualCP > cp;
                        case FilterComparisonType.LessThan:
                            return pokemon.ActualCP < cp;
                        case FilterComparisonType.GreaterThanOrEqual:
                        return pokemon.ActualCP >= cp;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.ActualCP <= cp;
                        case FilterComparisonType.NotEqual:
                            return pokemon.ActualCP != cp;
                        default:
                            return false;
                    }
                case PokemonFilterType.HP:
                    int hp;
                    test = int.TryParse(this.FilterValue, out hp);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.ActualHP == hp;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.ActualHP > hp;
                        case FilterComparisonType.LessThan:
                            return pokemon.ActualHP < hp;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.ActualHP >= hp;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.ActualHP <= hp;
                        case FilterComparisonType.NotEqual:
                            return pokemon.ActualHP != hp;
                        default:
                            return false;
                    }
                case PokemonFilterType.Attack:
                    int attackIV;
                    test = int.TryParse(this.FilterValue, out attackIV);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.GetAttackIV() == attackIV;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.GetAttackIV() > attackIV;
                        case FilterComparisonType.LessThan:
                            return pokemon.GetAttackIV() < attackIV;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.GetAttackIV() >= attackIV;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.GetAttackIV() <= attackIV;
                        case FilterComparisonType.NotEqual:
                            return pokemon.GetAttackIV() != attackIV;
                        default:
                            return false;
                    }
                case PokemonFilterType.Defense:
                    int defenseIV;
                    test = int.TryParse(this.FilterValue, out defenseIV);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.GetDefenseIV() == defenseIV;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.GetDefenseIV() > defenseIV;
                        case FilterComparisonType.LessThan:
                            return pokemon.GetDefenseIV() < defenseIV;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.GetDefenseIV() >= defenseIV;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.GetDefenseIV() <= defenseIV;
                        case FilterComparisonType.NotEqual:
                            return pokemon.GetDefenseIV() != defenseIV;
                        default:
                            return false;
                    }
                case PokemonFilterType.Stamina:
                    int staminaIV;
                    test = int.TryParse(this.FilterValue, out staminaIV);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.GetStaminaIV() == staminaIV;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.GetStaminaIV() > staminaIV;
                        case FilterComparisonType.LessThan:
                            return pokemon.GetStaminaIV() < staminaIV;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.GetStaminaIV() >= staminaIV;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.GetStaminaIV() <= staminaIV;
                        case FilterComparisonType.NotEqual:
                            return pokemon.GetStaminaIV() != staminaIV;
                        default:
                            return false;
                    }
                case PokemonFilterType.Level:
                    double level;
                    test = double.TryParse(this.FilterValue, out level);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return Math.Abs(pokemon.GetLevel() - level) <= 0.0001;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.GetLevel() > level;
                        case FilterComparisonType.LessThan:
                            return pokemon.GetLevel() < level;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.GetLevel() >= level;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.GetLevel() <= level;
                        case FilterComparisonType.NotEqual:
                            return Math.Abs(pokemon.GetLevel() - level) > 0.0001;
                        default:
                            return false;
                    }
                case PokemonFilterType.IVPercent:
                    double percentage;
                    test = double.TryParse(this.FilterValue, out percentage);
                    if (!test)
                        return false;
                    percentage = percentage / 100.0;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return Math.Abs(pokemon.IVPercentage - percentage) <= 0.0001;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.IVPercentage > percentage;
                        case FilterComparisonType.LessThan:
                            return pokemon.IVPercentage < percentage;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.IVPercentage >= percentage;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.IVPercentage <= percentage;
                        case FilterComparisonType.NotEqual:
                            return Math.Abs(pokemon.IVPercentage - percentage) > 0.0001;
                        default:
                            return false;
                    }
                case PokemonFilterType.PokedexNum:
                    int pokedexNum;
                    test = int.TryParse(this.FilterValue, out pokedexNum);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Species.Number == pokedexNum;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.Species.Number > pokedexNum;
                        case FilterComparisonType.LessThan:
                            return pokemon.Species.Number < pokedexNum;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.Species.Number >= pokedexNum;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.Species.Number <= pokedexNum;
                        case FilterComparisonType.NotEqual:
                            return pokemon.Species.Number != pokedexNum;
                        default:
                            return false;
                    }
                default:
                    return false;
            }            
        }
        #endregion
    }
}
