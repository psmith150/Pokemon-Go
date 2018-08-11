using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class PokemonMessage
    {
        public PokemonMessage(Pokemon pokemon)
        {
            this.Pokemon = pokemon;
        }
        public Pokemon Pokemon {get; private set;}
    }
}
