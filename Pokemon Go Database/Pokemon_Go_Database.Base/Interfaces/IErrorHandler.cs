using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Base.Interfaces
{
    public interface IErrorHandler
    {
        Task DisplayError(Exception e, Action callback = null);
        Task DisplayError(string message, Action callback = null);
    }
}
