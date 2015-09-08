using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAPI.Services.Entities
{
    [Table("CourseRegistrations")]
    class CourseRegistration
    {
        /// <summary>
        /// The ID of this registration
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The ID of the student
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        /// The ID of the course.
        /// </summary>
        public int CourseID { get; set; }

        /// <summary>
        /// Specifies whether or not this student is still
        /// in this class. If a student is removed from a course, 
        /// this is set to false.
        /// </summary>
        public bool Active { get; set; }
    }
}
