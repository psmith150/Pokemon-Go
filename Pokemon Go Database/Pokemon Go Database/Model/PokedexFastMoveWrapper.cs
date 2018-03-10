using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class PokedexFastMoveWrapper : ObservableObject
    {
        #region Constructor
        public PokedexFastMoveWrapper()
        {
            this.FastMove = new FastMove();
            this.IsLegacy = false;
        }
        public PokedexFastMoveWrapper(FastMove fastMove, bool isLegacy = false)
        {
            this.FastMove = fastMove;
            this.IsLegacy = isLegacy;
        }
        #endregion

        #region Public Properties
        private FastMove _FastMove;
        public FastMove FastMove
        {
            get
            {
                return _FastMove;
            }
            set
            {
                Set(ref this._FastMove, value);
            }
        }

        private bool _IsLegacy;
        public bool IsLegacy
        {
            get
            {
                return _IsLegacy;
            }
            set
            {
                Set(ref this._IsLegacy, value);
            }
        }
        #endregion
    }
}
