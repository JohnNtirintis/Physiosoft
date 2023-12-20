using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Physiosoft.CustomExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string username)
        : base($"User '{username}' not found.") { }
    }
}
