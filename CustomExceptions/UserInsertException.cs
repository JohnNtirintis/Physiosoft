using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Physiosoft.CustomExceptions
{
    public class UserInsertException : Exception
    {
        public UserInsertException() : base() { }

        public UserInsertException(string message) : base(message) { }

        public UserInsertException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
