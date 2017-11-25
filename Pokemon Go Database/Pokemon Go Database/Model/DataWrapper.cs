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
        public MyObservableCollection<Move> FastMoves { get; set; }   //Collection of fast moves
        public MyObservableCollection<Move> ChargeMoves { get; set; }   //Collection of charge moves
        public MyObservableCollection<PokedexEntry> PokedexEntries { get; set; }   //Collection of pokedex entries
    }
}
