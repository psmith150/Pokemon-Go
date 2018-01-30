using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pokemon_Go_Database.Screens
{
    public class TemplateViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        public TemplateViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
        }

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }
    }
}
