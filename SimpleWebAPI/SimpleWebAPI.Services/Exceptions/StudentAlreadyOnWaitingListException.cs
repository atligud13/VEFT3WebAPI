using System;

namespace SimpleWebAPI.Services.Exceptions
{
    /// <summary>
    /// Thrown when a client is trying to add a student
    /// on a waiting list for a course he is already on the waiting list for.
    /// </summary>
    public class StudentAlreadyOnWaitingListException : ApplicationException
    {
    }
}
