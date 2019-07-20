using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class PokedexFilterElement : ObservableObject
    {
        #region Constructor
        public PokedexFilterElement()
        {
            this.FilterType = PokedexFilterType.Name;
            this.ComparisonType = FilterComparisonType.Equal;
            this.FilterValue = "";
        }
        #endregion
        #region Public Properties
        private PokedexFilterType _FilterType;
        public PokedexFilterType FilterType
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
        public bool EvaluateFilter(PokedexEntry pokemon)
        {
            bool test = false;
            switch (this.FilterType)
            {
                case PokedexFilterType.Name:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Species.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.Species, this.FilterValue, true) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.Species, this.FilterValue, true) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.Species, this.FilterValue, true) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.Species, this.FilterValue, true) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !pokemon.Species.Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        default:
                            return false;
                    }
                case PokedexFilterType.PokedexNum:
                    int pokedexNum;
                    test = int.TryParse(this.FilterValue, out pokedexNum);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Number == pokedexNum;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.Number > pokedexNum;
                        case FilterComparisonType.LessThan:
                            return pokemon.Number < pokedexNum;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.Number >= pokedexNum;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.Number <= pokedexNum;
                        case FilterComparisonType.NotEqual:
                            return pokemon.Number != pokedexNum;
                        default:
                            return false;
                    }
                case PokedexFilterType.Type:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Type1.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.Type2.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.Type1.ToString(), this.FilterValue, true) >= 1 || string.Compare(pokemon.Type2.ToString(), this.FilterValue, true) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.Type1.ToString(), this.FilterValue, true) <= -1 || string.Compare(pokemon.Type2.ToString(), this.FilterValue, true) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.Type1.ToString(), this.FilterValue, true) >= 0 || string.Compare(pokemon.Type2.ToString(), this.FilterValue, true) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.Type1.ToString(), this.FilterValue, true) <= 0 || string.Compare(pokemon.Type2.ToString(), this.FilterValue, true) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !(pokemon.Type1.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.Type2.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase));
                        default:
                            return false;
                    }
                case PokedexFilterType.Move:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.FastMoves.Select(x => x.FastMove.Name).Contains(this.FilterValue, StringComparer.InvariantCultureIgnoreCase) || pokemon.ChargeMoves.Select(x => x.ChargeMove.Name).Contains(this.FilterValue, StringComparer.InvariantCultureIgnoreCase);
                        case FilterComparisonType.GreaterThan:
                            this.ComparisonType = FilterComparisonType.Equal;
                            return false;
                        case FilterComparisonType.LessThan:
                            this.ComparisonType = FilterComparisonType.Equal;
                            return false;
                        case FilterComparisonType.GreaterThanOrEqual:
                            this.ComparisonType = FilterComparisonType.Equal;
                            return false;
                        case FilterComparisonType.LessThanOrEqual:
                            this.ComparisonType = FilterComparisonType.Equal;
                            return false;
                        case FilterComparisonType.NotEqual:
                            return !(pokemon.FastMoves.Select(x => x.FastMove.Name).Contains(this.FilterValue, StringComparer.InvariantCultureIgnoreCase) || pokemon.ChargeMoves.Select(x => x.ChargeMove.Name).Contains(this.FilterValue, StringComparer.InvariantCultureIgnoreCase));
                        default:
                            return false;
                    }
                case PokedexFilterType.BaseAttack:
                    int attack;
                    test = int.TryParse(this.FilterValue, out attack);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Attack == attack;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.Attack > attack;
                        case FilterComparisonType.LessThan:
                            return pokemon.Attack < attack;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.Attack >= attack;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.Attack <= attack;
                        case FilterComparisonType.NotEqual:
                            return pokemon.Attack != attack;
                        default:
                            return false;
                    }
                case PokedexFilterType.BaseDefense:
                    int defense;
                    test = int.TryParse(this.FilterValue, out defense);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Defense == defense;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.Defense > defense;
                        case FilterComparisonType.LessThan:
                            return pokemon.Defense < defense;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.Defense >= defense;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.Defense <= defense;
                        case FilterComparisonType.NotEqual:
                            return pokemon.Defense != defense;
                        default:
                            return false;
                    }
                case PokedexFilterType.BaseStamina:
                    int stamina;
                    test = int.TryParse(this.FilterValue, out stamina);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Stamina == stamina;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.Stamina > stamina;
                        case FilterComparisonType.LessThan:
                            return pokemon.Stamina < stamina;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.Stamina >= stamina;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.Stamina <= stamina;
                        case FilterComparisonType.NotEqual:
                            return pokemon.Stamina != stamina;
                        default:
                            return false;
                    }
                case PokedexFilterType.MaxCP:
                    int cp;
                    test = int.TryParse(this.FilterValue, out cp);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.MaxCP == cp;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.MaxCP > cp;
                        case FilterComparisonType.LessThan:
                            return pokemon.MaxCP < cp;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.MaxCP >= cp;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.MaxCP <= cp;
                        case FilterComparisonType.NotEqual:
                            return pokemon.MaxCP != cp;
                        default:
                            return false;
                    }
                case PokedexFilterType.BaseStatTotal:
                    int total;
                    test = int.TryParse(this.FilterValue, out total);
                    if (!test)
                        return false;
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.TotalStats == total;
                        case FilterComparisonType.GreaterThan:
                            return pokemon.TotalStats > total;
                        case FilterComparisonType.LessThan:
                            return pokemon.TotalStats < total;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return pokemon.TotalStats >= total;
                        case FilterComparisonType.LessThanOrEqual:
                            return pokemon.TotalStats <= total;
                        case FilterComparisonType.NotEqual:
                            return pokemon.TotalStats != total;
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
