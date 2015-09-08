using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAPI.Services.Entities
{
    [Table("CourseTemplates")]
    class CourseTemplate
    {
        /// <summary>
        /// The database generated ID of the template
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Specifies the name of this course
        /// For example: Web Services
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the course ID of this course
        /// For example: T-514-VEFT
        /// </summary>
        public string CourseID { get; set; }
    }
}
