using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class IVCalculator : ObservableObject
    {
        public IVCalculator()
        {

        }

        #region Public Properties
        private bool _attackBest;
        private bool _defenseBest;
        private bool _staminaBest;
        #endregion
    }
}
