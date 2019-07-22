using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_Go_Database.Services;
using GalaSoft.MvvmLight;
using System.Reflection;

namespace Pokemon_Go_Database
{
    public abstract class BaseViewModel : ViewModelBase
    {
        private SessionService _Session;
        public SessionService Session
        {
            get
            {
                return this._Session;
            }
            private set
            {
                this.Set(ref this._Session, value);
            }
        }

        protected BaseViewModel(SessionService session)
        {
            this.Session = session;
        }
        public string AppVersion
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }
    }
}
