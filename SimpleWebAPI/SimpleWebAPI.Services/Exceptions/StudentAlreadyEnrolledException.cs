using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAPI.Services.Exceptions
{
    /// <summary>
    /// Thrown when a the client tries to add a student to a class he is already enrolled in.
    /// </summary>
    public class StudentAlreadyEnrolledException : ApplicationException
    {
    }
}
