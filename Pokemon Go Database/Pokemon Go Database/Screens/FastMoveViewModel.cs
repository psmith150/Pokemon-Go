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
    public class FastMoveViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        public FastMoveViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.FastMoves = this.Session.FastMoveList;
        }

        public override void Initialize()
        {

        }

        public override void Deinitialize()
        {

        }

        #region Public Properties
        private MyObservableCollection<FastMove> _fastMoves;
        public MyObservableCollection<FastMove> FastMoves
        {
            get
            {
                return this._fastMoves;
            }
            private set
            {
                this.Set(ref this._fastMoves, value);
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
