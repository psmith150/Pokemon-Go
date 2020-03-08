using Pokemon_Go_Database.Base.AbstractClasses;
using Pokemon_Go_Database.Services;
using System.Windows.Input;

namespace Pokemon_Go_Database.Screens
{
    public class PvpSimulationViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;
        #region Commands
        public ICommand SimulateBattleCommand { get; private set; }
        public ICommand SimulateAllPokemonCommand { get; private set; }
        public ICommand SimulateAllPokemonAt40Command { get; private set; }
        public ICommand IncrementPartySizeCommand { get; private set; }
        public ICommand DecrementPartySizeCommand { get; private set; }
        public ICommand AssignAttackerPokemonCommand { get; private set; }
        #endregion
        #region Constructior
        public PvpSimulationViewModel(NavigationService navigationService, SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this.navigationService = navigationService;
            this._messageViewer = messageViewer;
        }
        #endregion

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Private Fields
        private MessageViewerBase _messageViewer;
        #endregion

        #region Public Properties
        #endregion

        #region Private Methods
        #endregion
    }
}
