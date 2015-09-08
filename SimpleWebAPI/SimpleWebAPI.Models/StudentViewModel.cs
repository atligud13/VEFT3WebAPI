using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebAPI.Models
{
    /// <summary>
    /// A view model class for adding a new student.
    /// </summary>
    public class StudentViewModel
    {
        /// <summary>
        /// Social security number of this student
        /// </summary>
        [Required]
        public string SSN { get; set; }

        /// <summary>
        /// The name of this student
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
