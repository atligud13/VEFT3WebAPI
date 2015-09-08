using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAPI.Models
{
    public class CourseDetailsDTO
    {
        /// <summary>
        /// Name of the course
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Template ID of the course
        /// </summary>
        public string CourseID { get; set; }

        /// <summary>
        /// Database ID for this course
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Start date of this course
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of this course
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// A list of students enrolled in the course
        /// </summary>
        public List<StudentDTO> Students { get; set; }

        /// <summary>
        /// A number specifying the maximum number of students
        /// allowed to enroll in the course.
        /// </summary>
        public int MaxStudents { get; set; }
    }
}
