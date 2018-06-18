using Pokemon_Go_Database.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Base.EventArgs
{
    public class MessageViewerEventArgs
    {
        public MessageViewerResult Result { get; set; }

        public MessageViewerEventArgs(MessageViewerResult result = MessageViewerResult.Ok)
        {
            this.Result = result;
        }
    }
}
