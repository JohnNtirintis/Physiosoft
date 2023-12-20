using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Physiosoft.CustomExceptions
{
    public class InvalidLoginAttemptException : Exception
    {
        public InvalidLoginAttemptException()
        : base("Invalid login attempt.") { }
    }
}
