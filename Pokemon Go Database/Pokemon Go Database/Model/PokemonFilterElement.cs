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
    }
}
