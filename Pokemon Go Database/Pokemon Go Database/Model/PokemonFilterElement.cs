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
            switch (this.FilterType)
            {
                case PokemonFilterType.Name:
                    switch (this.ComparisonType)
                    {
                        case FilterComparisonType.Equal:
                            return pokemon.Name.Equals(this.FilterValue);
                        case FilterComparisonType.GreaterThan:
                            return string.Compare(pokemon.Name, this.FilterValue) >= 1;
                        case FilterComparisonType.LessThan:
                            return string.Compare(pokemon.Name, this.FilterValue) <= -1;
                        case FilterComparisonType.GreaterThanOrEqual:
                            return string.Compare(pokemon.Name, this.FilterValue) >= 0;
                        case FilterComparisonType.LessThanOrEqual:
                            return string.Compare(pokemon.Name, this.FilterValue) <= 0;
                        case FilterComparisonType.NotEqual:
                            return !pokemon.Name.Equals(this.FilterValue);
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
                //case PokemonFilterType.Type:
                //    switch (this.ComparisonType)
                //    {
                //        case FilterComparisonType.Equal:
                //            return pokemon.Species.Type1.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase) || pokemon.Species.Type2.ToString().Equals(this.FilterValue, StringComparison.InvariantCultureIgnoreCase);
                //        case FilterComparisonType.GreaterThan:
                //            return string.Compare(pokemon.Species.Type1.ToString(), this.FilterValue) >= 1;
                //        case FilterComparisonType.LessThan:
                //            return string.Compare(pokemon.Species.Species, this.FilterValue) <= -1;
                //        case FilterComparisonType.GreaterThanOrEqual:
                //            return string.Compare(pokemon.Species.Species, this.FilterValue) >= 0;
                //        case FilterComparisonType.LessThanOrEqual:
                //            return string.Compare(pokemon.Species.Species, this.FilterValue) <= 0;
                //        case FilterComparisonType.NotEqual:
                //            return !pokemon.Species.Species.Equals(this.FilterValue);
                //        default:
                //            return false;
                //    }
                default:
                    return false;
            }            
        }
        #endregion
    }
}
