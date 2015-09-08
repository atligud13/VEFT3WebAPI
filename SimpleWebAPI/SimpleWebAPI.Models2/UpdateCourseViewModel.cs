using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebAPI.Models
{
    /// <summary>
    /// A view model classing for updating an existing course.
    /// </summary>
    public class UpdateCourseViewModel
    {
        /// <summary>
        /// Start date of this course
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of this course
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// A number specifying the maximum number of students 
        /// allowed to enroll in the course.
        /// </summary>
        [Required]
        public int MaxStudents { get; set; }
    }
}
