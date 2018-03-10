using Pokemon_Go_Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Popups
{
    public class IVCalculatorPopupEventArgs : PopupEventArgs
    {
        public IVCalculatorPopupEventArgs(Pokemon pokemon)
        {
            this.NewPokemon = pokemon;
        }

        public Pokemon NewPokemon { get; set; }
    }
}
