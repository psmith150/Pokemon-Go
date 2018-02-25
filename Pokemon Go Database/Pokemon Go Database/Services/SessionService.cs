using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.IO;
using System.Xml.Serialization;
using Pokemon_Go_Database.Model;
using System.Diagnostics;
using SpreadsheetLight;

namespace Pokemon_Go_Database.Services
{
    public class SessionService : ObservableObject
    {
        public SessionService()
        {
            this.FastMoveList = new MyObservableCollection<FastMove>();
            this.ChargeMoveList = new MyObservableCollection<ChargeMove>();
            this.Pokedex = new MyObservableCollection<PokedexEntry>();
        }

        #region Public Properties

        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return this._IsBusy;
            }
            set
            {
                this.Set(ref this._IsBusy, value);
            }
        }
        private MyObservableCollection<FastMove> _fastMoveList;
        public MyObservableCollection<FastMove> FastMoveList
        {
            get
            {
                return _fastMoveList;
            }
            private set
            {
                this.Set(ref this._fastMoveList, value);
            }
        }

        private MyObservableCollection<ChargeMove> _chargeMoveList;
        public MyObservableCollection<ChargeMove> ChargeMoveList
        {
            get
            {
                return _chargeMoveList;
            }
            private set
            {
                this.Set(ref this._chargeMoveList, value);
            }
        }

        private MyObservableCollection<PokedexEntry> _pokedex;
        public MyObservableCollection<PokedexEntry> Pokedex
        {
            get
            {
                return _pokedex;
            }
            private set
            {
                this.Set(ref this._pokedex, value);
            }
        }
        #endregion

        #region Public Methods
        #region Saving and loading data
        public async Task SaveDataToFile(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.Moves = new MyObservableCollection<Move>();
                    data.Moves.InsertRange(this.FastMoveList);
                    data.Moves.InsertRange(this.ChargeMoveList);
                    data.PokedexEntries = this.Pokedex;

                    await Task.Run(() => dataSerializer.Serialize(stream, data)); //Saves the data using the attributes defined in each class
                }
            }
        }

        /// <summary>
        /// Loads data from the current filepath
        /// </summary>
        public async Task LoadDataFromFile(string filePath)
        {
            //Debug.WriteLine("Loading data");
            //Clears all existing data
            this.FastMoveList.Clear();
            this.ChargeMoveList.Clear();
            this.Pokedex.Clear();

            //Retrieves the data using the serialize attributes
            DataWrapper data = new DataWrapper();
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    data = await Task.Run(() => dataSerializer.Deserialize(file)) as DataWrapper;
                }
            }
            catch (IOException ex) //File does not exist; set everything to defaults
            {
                await CreateNewFile(Properties.Settings.Default.DefaultDirectory + "\\data_new.xml");
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Error reading xml:" + ex.Message);
            }
            //Process the data
            List<FastMove> tempFastMoves = new List<FastMove>();
            List<ChargeMove> tempChargeMoves = new List<ChargeMove>();
            foreach (Move move in data.Moves)
            {
                if (move.MoveType == MoveType.Fast)
                {
                    tempFastMoves.Add(move as FastMove);
                }
                else if (move.MoveType == MoveType.Charge)
                {
                    tempChargeMoves.Add(move as ChargeMove);
                }
                else
                {
                    throw new InvalidOperationException("Unknown move type: " + move.MoveType);
                }
            }
            this.FastMoveList.InsertRange(tempFastMoves);
            this.ChargeMoveList.InsertRange(tempChargeMoves);
            List<PokedexEntry> tempPokedex = new List<PokedexEntry>();
            foreach (PokedexEntry species in data.PokedexEntries)
            {
                //Debug.WriteLine($"{species.Species} has {species.FastMoves.Found fast move {move.Name} for {species.Species}");
                for (int i = 0; i < species.FastMoves.Count; i++)
                {
                    //Debug.WriteLine($"Found fast move {move.Name} for {species.Species}");
                    PokedexFastMoveWrapper move = species.FastMoves[i];
                    string moveName = move.FastMove.Name;
                    try
                    {
                        move.FastMove = (this.FastMoveList.Single(x => x.Name.Equals(moveName)));
                        species.FastMoves[i] = move;
                    }
                    catch (ArgumentException ex)
                    {
                        throw new ArgumentException("Cannot find matching fast move " + move.FastMove.Name + " in fast move list.", ex);
                    }
                }
                for (int i = 0; i < species.ChargeMoves.Count; i++)
                {
                    //Debug.WriteLine($"Found fast move {move.Name} for {species.Species}");
                    PokedexChargeMoveWrapper move = species.ChargeMoves[i];
                    string moveName = move.ChargeMove.Name;
                    try
                    {
                        move.ChargeMove = (this.ChargeMoveList.Single(x => x.Name.Equals(moveName)));
                        species.ChargeMoves[i] = move;
                    }
                    catch (ArgumentException ex)
                    {
                        throw new ArgumentException("Cannot find matching charge move " + move.ChargeMove.Name + " in charge move list.", ex);
                    }
                }
                tempPokedex.Add(species);
                this.Pokedex.Add(species);
            }
            //this.Pokedex.InsertRange(tempPokedex);

            //Debug.WriteLine($"Loaded {data.FastMoves.Count} fast moves, {data.ChargeMoves.Count} charge moves, and {Pokedex.Count} pokemon");
            //this.FastMoveList.Add(new Move())
        }

        /// <summary>
        /// Creates a new data file with no data in the current filepath
        /// </summary>
        public async Task CreateNewFile(string filePath)
        {
            //Debug.WriteLine("Creating new file at " + filePath);
            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.Moves = new MyObservableCollection<Move>();
                    data.PokedexEntries = new MyObservableCollection<PokedexEntry>();

                    await Task.Run(() => dataSerializer.Serialize(stream, data)); //Saves the data using the attributes defined in each class
                }
            }
        }
        #endregion

        #region Excel Handling
        public async Task ReadExcelData(string filePath)
        {
            SLDocument document = new SLDocument(filePath);
            await ReadMovesetData(document);
        }

        #endregion
        #endregion

        #region Private Methods
        private async Task ReadMovesetData(SLDocument document)
        {
            document.SelectWorksheet("Movesets");
            string test = document.GetCellValueAsString(1,1);
            int row = 2, col = 2;
            while (!string.IsNullOrWhiteSpace(document.GetCellValueAsString(row, col)))
            {
                col = 2;
                string name = document.GetCellValueAsString(row, col);
                PokedexEntry species = this.Pokedex.Single(x => x.Species.Equals(name));
                if (species == null)
                {
                    Debug.WriteLine($"Could not find species {name}");
                    row++;
                    continue;
                }
                species.FastMoves.Clear();
                species.ChargeMoves.Clear();
                for (int i = 0; i < 2; i++)
                {
                    string moveName = document.GetCellValueAsString(row, 3 + i);
                    if (!string.IsNullOrWhiteSpace(moveName))
                    {
                        FastMove fastMove = await Task.Run(() => this.FastMoveList.Single(x => x.Name.Equals(moveName)));
                        if (fastMove != null)
                            species.FastMoves.Add(new PokedexFastMoveWrapper(fastMove));
                    }
                }
                int index = 0;
                while (!string.IsNullOrWhiteSpace(document.GetCellValueAsString(row, 5 + index)))
                {
                    string moveName = document.GetCellValueAsString(row, 5 + index);
                    ChargeMove chargeMove = await Task.Run(() => this.ChargeMoveList.Single(x => x.Name.Equals(moveName)));
                    if (chargeMove != null)
                        species.ChargeMoves.Add(new PokedexChargeMoveWrapper(chargeMove));
                    index++;
                }
                row++;
            }
        }
        #endregion
    }
}
