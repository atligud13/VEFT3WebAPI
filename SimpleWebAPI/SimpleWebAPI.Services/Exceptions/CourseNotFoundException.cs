using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAPI.Services.Exceptions
{
    /// <summary>
    /// Thrown when a course is not found.
    /// </summary>
    public class CourseNotFoundException : ApplicationException
    {
    }
}
