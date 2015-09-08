using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebAPI.Models
{
    /// <summary>
    /// A view model for adding an existing student
    /// to a course or the waiting list for a course.
    /// </summary>
    public class AddStudentToCourseViewModel
    {
        /// <summary>
        /// Social security number of this student
        /// </summary>
        [Required]
        public string SSN { get; set; }
    }
}
