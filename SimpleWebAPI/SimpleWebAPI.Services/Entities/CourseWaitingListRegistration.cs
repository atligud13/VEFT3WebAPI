using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleWebAPI.Services.Entities
{
    [Table("CourseWaitingList")]
    class CourseWaitingListRegistration
    {
        /// <summary>
        /// The database generated ID of the entry
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The database ID of the course this student
        /// is assigned to the waiting list for.
        /// </summary>
        public int CourseID { get; set; }

        /// <summary>
        /// The database generated ID of the student.
        /// </summary>
        public int StudentID { get; set; }
    }
}
