using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAPI.Services.Entities
{
    [Table("Persons")]
    class Person
    {
        /// <summary>
        /// Database ID for this person
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Social security number of this person
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The name of this person
        /// </summary>
        public string Name { get; set; }
    }
}
