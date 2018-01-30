using GalaSoft.MvvmLight;
using Pokemon_Go_Database.IOC;
using Pokemon_Go_Database.Popups;
using Pokemon_Go_Database.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Services
{
    public class NavigationService : ObservableObject
    {
        private TaskCompletionSource<PopupEventArgs> popupTaskCompletionSource;

        private ScreenViewModel _ActiveViewModel;
        public ScreenViewModel ActiveViewModel
        {
            get
            {
                return this._ActiveViewModel;
            }
            protected set
            {
                this.Set(ref this._ActiveViewModel, value);
            }
        }

        private PopupViewModel _ActivePopupViewModel;
        public PopupViewModel ActivePopupViewModel
        {
            get
            {
                return this._ActivePopupViewModel;
            }
            protected set
            {
                this.Set(ref this._ActivePopupViewModel, value);
                this.RaisePropertyChanged(nameof(this.IsPopupActive));
            }
        }

        public bool IsPopupActive => this.ActivePopupViewModel != null;

        public void NavigateTo<TViewModel>()
            where TViewModel : ScreenViewModel
        {
            this.NavigateTo(typeof(TViewModel));
        }

        public void NavigateTo(Type viewModelType)
        {
            // if there is currently an active view model, deinitialize it
            this.ActiveViewModel?.Deinitialize();

            // set the active view model to be the specified screen and initialize it
            var nextViewModel = IoCContainer.ResolveViewModel(viewModelType) as ScreenViewModel;
            if (nextViewModel != null)
            {
                this.ActiveViewModel = nextViewModel;
                this.ActiveViewModel.Initialize();
            }
            else
            {
                this.ActiveViewModel = null;
            }
        }

        public Task<PopupEventArgs> OpenPopup<TViewModel>(object param = null)
            where TViewModel : PopupViewModel
        {
            return this.OpenPopup(typeof(TViewModel), param);
        }

        public Task<PopupEventArgs> OpenPopup(Type viewModelType, object param = null)
        {
            this.ActivePopupViewModel?.Deinitialize();

            var nextViewModel = IoCContainer.ResolveViewModel(viewModelType) as PopupViewModel;

            if (nextViewModel != null)
            {
                this.popupTaskCompletionSource = new TaskCompletionSource<PopupEventArgs>();

                this.ActivePopupViewModel = nextViewModel;
                this.ActivePopupViewModel.Closed += this.ActivePopupViewModel_Closed;
                this.ActivePopupViewModel.Initialize(param);

                return this.popupTaskCompletionSource.Task;
            }
            else
            {
                this.ActivePopupViewModel = null;
            }

            return Task.FromResult<PopupEventArgs>(null);
        }

        private void ActivePopupViewModel_Closed(object sender, PopupEventArgs e)
        {
            this.ActivePopupViewModel.Closed -= this.ActivePopupViewModel_Closed;
            this.ActivePopupViewModel?.Deinitialize();
            this.ActivePopupViewModel = null;
            this.popupTaskCompletionSource.SetResult(e);
        }
    }
}
