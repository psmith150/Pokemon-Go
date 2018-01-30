using Pokemon_Go_Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Screens
{
    public abstract class ScreenViewModel : BaseViewModel
    {
        public ScreenViewModel(SessionService session) : base(session) { }

        public abstract void Initialize();

        public abstract void Deinitialize();
    }
}
