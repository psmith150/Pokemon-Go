using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Pokemon_Go_Database.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        //Data used for file management
        private String fileName = "Pokemon_Go_Data";   //Name of the data files: <fileName>_<_currentYear>.xml
        private String filePath = "D:\\OneDrive\\Documents\\Miscellaneous\\";   //File path of the data files.
        private String completeFilePath;    //Complete file path

        private MyObservableCollection<FastMove> _fastMoveList; //The list of all valid fast moves
        private MyObservableCollection<ChargeMove> _chargeMoveList; //The list of all valid charge moves
        private MyObservableCollection<PokedexEntry> _pokedex; //The pokedex entries for all pokemon

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            completeFilePath = filePath + fileName + ".xml";

            _fastMoveList = new MyObservableCollection<FastMove>();
            _chargeMoveList = new MyObservableCollection<ChargeMove>();
            _pokedex = new MyObservableCollection<PokedexEntry>();

            SaveDataCommand = new RelayCommand(() => SaveData());

            Debug.WriteLine("Loading data from " + completeFilePath);
            LoadData();
            Debug.WriteLine("Number of fast moves is " + _fastMoveList.Count);
            //_fastMoveList.Add(new Move("Test move"));
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}

        public MyObservableCollection<FastMove> FastMoveList
        {
            get
            {
                return _fastMoveList;
            }
        }

        public MyObservableCollection<ChargeMove> ChargeMoveList
        {
            get
            {
                return _chargeMoveList;
            }
        }

        public  MyObservableCollection<PokedexEntry> Pokedex
        {
            get
            {
                return _pokedex;
            }
        }

        #region Saving and loading data

        /// <summary>
        /// Command to save the current data.
        /// </summary>
        public RelayCommand SaveDataCommand
        {
            get; set;
        }
        /// <summary>
        /// Saves the current data in the current filepath
        /// </summary>
        public void SaveData()
        {
            using (FileStream file = new FileStream(completeFilePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.FastMoves = _fastMoveList;
                    data.ChargeMoves = _chargeMoveList;
                    data.PokedexEntries = _pokedex;

                    dataSerializer.Serialize(stream, data); //Saves the data using the attributes defined in each class
                }
            }
        }

        /// <summary>
        /// Loads data from the current filepath
        /// </summary>
        public void LoadData()
        {
            //Debug.WriteLine("Loading data");
            //Clears all existing data
            _fastMoveList.Clear();
            _chargeMoveList.Clear();
            _pokedex.Clear();

            //Retrieves the data using the serialize attributes
            DataWrapper data = new DataWrapper();
            try
            {
                using (FileStream file = new FileStream(completeFilePath, FileMode.Open))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    data = (DataWrapper)dataSerializer.Deserialize(file);
                }
            }
            catch (Exception ex ) //File does not exist; set everything to defaults
            {
                InitNewFile();
            }
            //Process the data
            int index = 0;
            //Add groups, categories, and budget values
            _fastMoveList = data.FastMoves;
            _chargeMoveList = data.ChargeMoves;
            _pokedex = data.PokedexEntries;
        }

        /// <summary>
        /// Creates a new data file with no data in the current filepath
        /// </summary>
        private void InitNewFile()
        {
            Debug.WriteLine("Creating new file at " + completeFilePath);
            using (FileStream file = new FileStream(completeFilePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.FastMoves = new MyObservableCollection<FastMove>();
                    data.ChargeMoves = new MyObservableCollection<ChargeMove>();
                    data.PokedexEntries = new MyObservableCollection<PokedexEntry>();

                    dataSerializer.Serialize(stream, data); //Saves the data using the attributes defined in each class
                }
            }
        }
        #endregion
    }
}