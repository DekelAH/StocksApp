using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class FinnhubExceptionHandler : Exception
    {
        #region Ctors

        public FinnhubExceptionHandler() : base()
        {
            
        }

        public FinnhubExceptionHandler(string? message) : base(message)
        {
            
        }

        public FinnhubExceptionHandler(string? message, Exception? innerException) : base(message, innerException)
        {
            
        }

        #endregion
    }
}
