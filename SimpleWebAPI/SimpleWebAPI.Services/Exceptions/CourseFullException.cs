using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAPI.Services.Exceptions
{
    /// <summary>
    /// Thrown when the client tries to add a student to a course
    /// that already has the maximum number of students enrolled in it.
    /// </summary>
    public class CourseFullException : ApplicationException
    {
    }
}
