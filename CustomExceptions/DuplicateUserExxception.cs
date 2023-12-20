using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Physiosoft.CustomExceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string identifier)
        : base($"A user with the identifier '{identifier}' already exists.") { }
    }
}
