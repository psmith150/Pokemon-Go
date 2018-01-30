using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pokemon_Go_Database.Model
{
    /// <summary>
    /// Wrapper class for saving and loading data
    /// </summary>
    [Serializable]
    [XmlRoot("Data")]
    public class DataWrapper
    {
        public MyObservableCollection<Move> Moves { get; set; }   //Collection of all moves
        public MyObservableCollection<PokedexEntry> PokedexEntries { get; set; }   //Collection of pokedex entries
    }
}
