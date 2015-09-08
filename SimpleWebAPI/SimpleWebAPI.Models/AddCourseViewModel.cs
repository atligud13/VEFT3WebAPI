using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebAPI.Models
{
    /// <summary>
    /// A view model class for adding a new course
    /// </summary>
    public class AddCourseViewModel
    {
        
        /// <summary>
        /// Course ID of this course
        /// For example: T-514-VEFT
        /// </summary>
        [Required]
        public string CourseID { get; set; }

        /// <summary>
        /// Specifies which semester this particular course is in.
        /// For example: 20153 would mean that it the course is 
        /// from the year 2015, and it's the third semester of the year.
        /// With Spring being first, summer second and autumn third, 
        /// 20153 would mean Autumn 2015.
        /// </summary>
        [Required]
        public string Semester { get; set; }

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
