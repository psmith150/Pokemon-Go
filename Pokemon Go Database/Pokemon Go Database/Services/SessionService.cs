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
            this.MyPokemon = new MyObservableCollection<Pokemon>();
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

        private MyObservableCollection<Pokemon> _MyPokemon;
        public MyObservableCollection<Pokemon> MyPokemon
        {
            get
            {
                return _MyPokemon;
            }
            private set
            {
                this.Set(ref this._MyPokemon, value);
            }
        }
        #endregion

        #region Public Methods
        #region Saving and loading data
        public async Task SaveUserDataToFile(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(UserDataWrapper));
                    UserDataWrapper data = new UserDataWrapper();
                    data.Pokemon = this.MyPokemon;

                    await Task.Run(() => dataSerializer.Serialize(stream, data)); //Saves the data using the attributes defined in each class
                }
            }
        }

        public async Task SaveBaseDataToFile(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(BaseDataWrapper));
                    BaseDataWrapper data = new BaseDataWrapper();
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
        public async Task LoadUserDataFromFile(string filePath)
        {
            this.MyPokemon.Clear();
            UserDataWrapper data = new UserDataWrapper();
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(UserDataWrapper));
                    data = await Task.Run(() => dataSerializer.Deserialize(file)) as UserDataWrapper;
                }
            }
            catch (IOException) //File does not exist; set everything to defaults
            {
                //await CreateNewFile(Properties.Settings.Default.DefaultDirectory + "\\data_new.xml");
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Error reading xml:" + ex.Message);
            }
            List<Pokemon> tempPokemon = new List<Pokemon>();
            foreach (Pokemon pokemon in data.Pokemon)
            {
                try
                {
                    pokemon.Species = this.Pokedex.Single(x => x.Species.Equals(pokemon.Species.Species));
                }
                catch (Exception)
                {
                    throw new ArgumentException("Cannot find matching species " + pokemon.Species.Species + " in Pokedex.");
                }
                try
                {
                    pokemon.FastMove = pokemon.Species.FastMoves.Single(x => x.FastMove.Name.Equals(pokemon.FastMove.FastMove.Name));
                }
                catch (Exception)
                {
                    if (pokemon.FastMove != null)
                        throw new ArgumentException($"Cannot find matching fast move {pokemon.FastMove.FastMove.Name} in fast move list for {pokemon.Species.Species}");
                    else
                        throw new ArgumentException($"Fast move not defined for {pokemon.Name}");
                }
                try
                {
                    pokemon.ChargeMove = pokemon.Species.ChargeMoves.Single(x => x.ChargeMove.Name.Equals(pokemon.ChargeMove.ChargeMove.Name));
                }
                catch (Exception)
                {
                    if (pokemon.ChargeMove != null)
                        throw new ArgumentException($"Cannot find matching charge move {pokemon.ChargeMove.ChargeMove.Name} in fast move list for {pokemon.Species.Species}");
                    else
                        throw new ArgumentException($"Cannot find matching fast move {pokemon.ChargeMove.ChargeMove.Name} in fast move list for {pokemon.Name}");
                }
                try
                {
                    tempPokemon.Add(pokemon);
                }
                catch (Exception)
                {
                    string temp = pokemon.Name;
                }
            }
            this.MyPokemon.InsertRange(tempPokemon);
        }

        public async Task LoadBaseDataFromFile(string filePath)
        {
            this.FastMoveList.Clear();
            this.ChargeMoveList.Clear();
            this.Pokedex.Clear();

            //Retrieves the data using the serialize attributes
            BaseDataWrapper data = new BaseDataWrapper();
            FileStream file = null;
            try
            {
                file = new FileStream(filePath, FileMode.Open);
            }
            catch (IOException) //File does not exist; set everything to defaults
            {
                Debug.WriteLine("Error reading base data");
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Error reading xml:" + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading xml: " + ex.Message);
            }
            await this.LoadBaseDataFromFile(file);
        }
        public async Task LoadBaseDataFromFile(Stream fileStream)
        {
            this.FastMoveList.Clear();
            this.ChargeMoveList.Clear();
            this.Pokedex.Clear();

            //Retrieves the data using the serialize attributes
            BaseDataWrapper data = new BaseDataWrapper();
            try
            {
                XmlSerializer dataSerializer = new XmlSerializer(typeof(BaseDataWrapper));
                data = await Task.Run(() => dataSerializer.Deserialize(fileStream)) as BaseDataWrapper;
            }
            catch (IOException) //File does not exist; set everything to defaults
            {
                Debug.WriteLine("Error reading base data");
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Error reading xml:" + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading xml: " + ex.Message);
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
            this.FastMoveList.InsertRange(tempFastMoves.OrderBy(x => x.Name));
            this.ChargeMoveList.InsertRange(tempChargeMoves.OrderBy(x => x.Name));
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
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(UserDataWrapper));
                    UserDataWrapper data = new UserDataWrapper();
                    //data.Moves = new MyObservableCollection<Move>();
                    //data.PokedexEntries = new MyObservableCollection<PokedexEntry>();

                    await Task.Run(() => dataSerializer.Serialize(stream, data)); //Saves the data using the attributes defined in each class
                }
            }
        }
        #endregion

        #region Excel Handling
        public async Task ReadExcelData(string filePath)
        {
            SLDocument document = new SLDocument(filePath);
            //await ReadMovesetExcelData(document);
            await ReadPokemonExcelData(document);
        }

        #endregion
        #endregion

        #region Private Methods
        private async Task ReadMovesetExcelData(SLDocument document)
        {
            document.SelectWorksheet("Movesets");
            int row = 2, col = 2;
            while (!string.IsNullOrWhiteSpace(document.GetCellValueAsString(row, col)))
            {
                col = 2;
                string name = document.GetCellValueAsString(row, col);
                PokedexEntry species = this.Pokedex.SingleOrDefault(x => x.Species.Equals(name));
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
                        FastMove fastMove = await Task.Run(() => this.FastMoveList.SingleOrDefault(x => x.Name.Equals(moveName)));
                        if (fastMove != null)
                            species.FastMoves.Add(new PokedexFastMoveWrapper(fastMove));
                    }
                }
                int index = 0;
                while (!string.IsNullOrWhiteSpace(document.GetCellValueAsString(row, 5 + index)))
                {
                    string moveName = document.GetCellValueAsString(row, 5 + index);
                    ChargeMove chargeMove = await Task.Run(() => this.ChargeMoveList.SingleOrDefault(x => x.Name.Equals(moveName)));
                    if (chargeMove != null)
                        species.ChargeMoves.Add(new PokedexChargeMoveWrapper(chargeMove));
                    index++;
                }
                row++;
            }
        }

        private async Task ReadPokemonExcelData(SLDocument document)
        {
            document.SelectWorksheet("My Pokemon");
            int row = 2, col = 2;
            this.MyPokemon.Clear();
            while (!string.IsNullOrWhiteSpace(document.GetCellValueAsString(row, col)))
            {
                col = 2;
                string name = document.GetCellValueAsString(row, 2);
                string speciesName = document.GetCellValueAsString(row, 3);
                PokedexEntry species = this.Pokedex.SingleOrDefault(x => x.Species.Equals(speciesName));
                if (species == null)
                {
                    Debug.WriteLine($"Could not find species {speciesName}");
                    row++;
                    continue;
                }
                Pokemon pokemon = new Pokemon(species, name);

                pokemon.GameCP = document.GetCellValueAsInt32(row, 8);
                pokemon.GameHP = document.GetCellValueAsInt32(row, 9);
                pokemon.DustToPower = document.GetCellValueAsInt32(row, 10);
                pokemon.HasBeenPowered = document.GetCellValueAsString(row, 11).Equals("Yes");
                pokemon.StaminaIVExpression = document.GetCellValueAsString(row, 15);
                pokemon.AttackIVExpression = document.GetCellValueAsString(row, 16);
                pokemon.DefenseIVExpression = document.GetCellValueAsString(row, 17);
                pokemon.LevelExpression = document.GetCellValueAsString(row, 18);

                string fastMoveName = document.GetCellValueAsString(row, 13);
                PokedexFastMoveWrapper fastMove = await Task.Run(() => species.FastMoves.SingleOrDefault(x => x.FastMove.Name.Equals(fastMoveName)));
                if (fastMove != null)
                    pokemon.FastMove = fastMove;
                string chargeMoveName = document.GetCellValueAsString(row, 14);
                PokedexChargeMoveWrapper chargeMove = await Task.Run(() => species.ChargeMoves.SingleOrDefault(x => x.ChargeMove.Name.Equals(chargeMoveName)));
                if (chargeMove != null)
                    pokemon.ChargeMove = chargeMove;

                this.MyPokemon.Add(pokemon);
                row++;
            }
        }
        #endregion
    }
}
