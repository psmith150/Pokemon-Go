using Pokemon_Go_Database.Base.Enums;
using Pokemon_Go_Database.Base.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using System.Windows.Media.Imaging;

namespace Pokemon_Go_Database.Base.AbstractClasses
{
    public abstract class MessageViewerBase : ObservableObject
    {
        #region Commands
        public ICommand CloseOkCommand { get; protected set; }
        public ICommand CloseCancelCommand { get; protected set; }
        #endregion

        #region Public Properties
        private bool _IsMessageActive;
        public bool IsMessageActive
        {
            get
            {
                return this._IsMessageActive;
            }
            set
            {
                this.Set(ref this._IsMessageActive, value);
            }
        }

        private string _Title;
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this.Set(ref this._Title, value);
            }
        }

        private string _ActiveMessage;
        public string ActiveMessage
        {
            get
            {
                return this._ActiveMessage;
            }
            set
            {
                this.Set(ref this._ActiveMessage, value);
            }
        }

        private bool _UseOkButton;
        public bool UseOkButton
        {
            get
            {
                return this._UseOkButton;
            }
            set
            {
                this.Set(ref this._UseOkButton, value);
            }
        }

        private bool _UseCancelButton;
        public bool UseCancelButton
        {
            get
            {
                return this._UseCancelButton;
            }
            set
            {
                this.Set(ref this._UseCancelButton, value);
            }
        }

        private BitmapImage _Icon;
        public BitmapImage Icon
        {
            get
            {
                return this._Icon;
            }
            set
            {
                this.Set(ref this._Icon, value);
            }
        }
        #endregion

        #region Public Methods
        public abstract Task<MessageViewerEventArgs> DisplayMessage(string message);
        public abstract Task<MessageViewerEventArgs> DisplayMessage(string message, string title);
        public abstract Task<MessageViewerEventArgs> DisplayMessage(string message, string title, MessageViewerButton button);
        public abstract Task<MessageViewerEventArgs> DisplayMessage(string message, string title, MessageViewerButton button, MessageViewerIcon icon);
        #endregion
    }
}
