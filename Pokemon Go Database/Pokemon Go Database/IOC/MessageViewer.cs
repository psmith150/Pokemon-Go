using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_Go_Database.Base.AbstractClasses;
using Pokemon_Go_Database.Base.Enums;
using Pokemon_Go_Database.Base.EventArgs;
using Pokemon_Go_Database.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Media.Imaging;

namespace Pokemon_Go_Database.IOC
{
    class MessageViewer : MessageViewerBase
    {
        #region Constructors

        public MessageViewer(NavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.CloseOkCommand = new RelayCommand(() => this.MessageClosed_Ok());
            this.CloseCancelCommand = new RelayCommand(() => this.MessageClosed_Cancel());
        }

        #endregion

        #region Private Fields
        private TaskCompletionSource<MessageViewerEventArgs> messageTaskCompletionSource;
        #endregion

        #region Public Properties

        #endregion

        #region Private Fields

        private NavigationService NavigationService;

        #endregion

        #region Public Methods

        public override Task<MessageViewerEventArgs> DisplayMessage(string message)
        {
            return this.DisplayMessage(message, "Message");
        }

        public override Task<MessageViewerEventArgs> DisplayMessage(string message, string title)
        {
            return this.DisplayMessage(message, title, MessageViewerButton.Ok);
        }

        public override Task<MessageViewerEventArgs> DisplayMessage(string message, string title, MessageViewerButton button)
        {
            return this.DisplayMessage(message, title, button, MessageViewerIcon.Information);
        }

        public override Task<MessageViewerEventArgs> DisplayMessage(string message, string title, MessageViewerButton button, MessageViewerIcon icon)
        {
            this.ActiveMessage = message;
            this.Title = title;
            this.Icon = new BitmapImage();
            switch (icon)
            {
                case MessageViewerIcon.Information:
                    this.Icon = new BitmapImage(new Uri(@"/Images\StatusInformation_16x.png", UriKind.Relative));
                    break;
                case MessageViewerIcon.Warning:
                    this.Icon = new BitmapImage(new Uri(@"/Images\StatusWarning_16x.png", UriKind.Relative));
                    break;
                case MessageViewerIcon.Error:
                    this.Icon = new BitmapImage(new Uri(@"/Images\StatusCriticalError_16x.png", UriKind.Relative));
                    break;
                default:
                    this.Icon = new BitmapImage(new Uri(@"/Images\StatusInformation_16x.png", UriKind.Relative));
                    break;
            }
            this.UseOkButton = false;
            this.UseCancelButton = false;
            switch (button)
            {
                case MessageViewerButton.Ok:
                    this.UseOkButton = true;
                    break;
                case MessageViewerButton.OkCancel:
                    this.UseOkButton = true;
                    this.UseCancelButton = true;
                    break;
                default:
                    this.UseOkButton = true;
                    break;
            }
            this.IsMessageActive = true;
            this.messageTaskCompletionSource = new TaskCompletionSource<MessageViewerEventArgs>();
            return this.messageTaskCompletionSource.Task;
        }

        #endregion

        #region Private Methods
        private void CloseMessageViewer(MessageViewerEventArgs e)
        {
            this.ActiveMessage = "";
            this.IsMessageActive = false;
            this.messageTaskCompletionSource.SetResult(e);
        }

        private void MessageClosed_Ok()
        {
            this.CloseMessageViewer(new MessageViewerEventArgs(MessageViewerResult.Ok));
        }

        private void MessageClosed_Cancel()
        {
            this.CloseMessageViewer(new MessageViewerEventArgs(MessageViewerResult.Cancel));
        }
        #endregion
    }
}
