using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_Go_Database.Services;
using System.Windows.Data;
using Pokemon_Go_Database.Model;

namespace Pokemon_Go_Database.Screens
{
    public class ChargeMoveViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        public ChargeMoveViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.ChargeMoves = this.Session.ChargeMoveList;
        }

        public override void Initialize()
        {

        }

        public override void Deinitialize()
        {

        }

        #region Public Properties
        private MyObservableCollection<ChargeMove> _chargeMoves;
        public MyObservableCollection<ChargeMove> ChargeMoves
        {
            get
            {
                return this._chargeMoves;
            }
            private set
            {
                this.Set(ref this._chargeMoves, value);
            }
        }
        public Array Types
        {
            get
            {
                return Enum.GetValues(typeof(Model.Type));
            }
        }
        #endregion
    }
}
