using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Base.AbstractClasses;
using Pokemon_Go_Database.Base.Enums;
using Pokemon_Go_Database.Base.EventArgs;
using Pokemon_Go_Database.Model;
using Pokemon_Go_Database.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace Pokemon_Go_Database.Popups
{
    public class SettingsViewModel : PopupViewModel
    {
        #region Commands
        public ICommand ExitPopupCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SelectDirectoryCommand { get; private set; }
        #endregion

        public SettingsViewModel(SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this._messageViewer = messageViewer;

            this.ExitPopupCommand = new RelayCommand(() => Exit());
            this.SaveCommand = new RelayCommand(() => Save());
            this.SelectDirectoryCommand = new RelayCommand(() => this.SelectDirectory());
        }

        
        public override void Initialize(object param)
        {
            this.DefaultDirectory = Properties.Settings.Default.DefaultDirectory;
            this.NameFormat = Properties.Settings.Default.SuggestedName;
            this._savingNeeded = false;
        }

        public override void Deinitialize()
        {
        }

        #region Private Fields
        private bool _savingNeeded = false;
        private MessageViewerBase _messageViewer;
        #endregion

        #region Public Properties
        private string _DefaultDirectory;
        public string DefaultDirectory
        {
            get
            {
                return this._DefaultDirectory;
            }
            set
            {
                this.Set(ref this._DefaultDirectory, value);
                this._savingNeeded = true;
            }
        }

        private string _NameFormat;
        public string NameFormat
        {
            get
            {
                return this._NameFormat;
            }
            set
            {
                this.Set(ref this._NameFormat, value);
                this._savingNeeded = true;
            }
        }
        #endregion

        #region Private Methods
        private void SelectDirectory()
        {
            FolderBrowserDialog dirDialog = new FolderBrowserDialog();
            dirDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dirDialog.ShowNewFolderButton = true;
            dirDialog.Description = "Select a folder";
            DialogResult result = dirDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.DefaultDirectory = dirDialog.SelectedPath;
            }
        }
        private async void Exit()
        {
            if (this._savingNeeded)
            {
                MessageViewerEventArgs result = await this._messageViewer.DisplayMessage("Settings have not been saved, are you sure you want to exit?", "Discard Changes?", MessageViewerButton.OkCancel);
                if (result.Result == MessageViewerResult.Ok)
                    this.ClosePopup(null);
            }
            else
            {
                this.ClosePopup(null);
            }
        }

        private void Save()
        {
            //TODO: implement saving
            Properties.Settings.Default.DefaultDirectory = this.DefaultDirectory;
            Properties.Settings.Default.SuggestedName = this.NameFormat;
            Properties.Settings.Default.Save();
            this.ClosePopup(null);
        }
        #endregion
    }
}
