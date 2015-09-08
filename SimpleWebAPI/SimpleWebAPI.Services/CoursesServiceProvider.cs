using System;
using System.Collections.Generic;
using System.Linq;
using SimpleWebAPI.Models;
using SimpleWebAPI.Services.Repositories;
using SimpleWebAPI.Services.Exceptions;
using SimpleWebAPI.Services.Entities;

namespace SimpleWebAPI.Services
{
    /// <summary>
    /// Service provider around courses and students enrolled in them
    /// </summary>
    public class CoursesServiceProvider
    {
        private readonly AppDataContext _db;

        /// <summary>
        /// Constructor
        /// </summary>
        public CoursesServiceProvider()
        {
            _db = new AppDataContext();
        }

        #region Courses
        /// <summary>
        /// Returns a list of courses for a given semester.
        /// If no semester is provided, the current semester
        /// will be provided.
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public List<CourseDTO> GetCoursesBySemester(string semester = null)
        {
            // Setting to default semester if empty
            if (string.IsNullOrEmpty(semester))
            {
                semester = "20153";
            }

            // Building the return object
            var result = (from c in _db.Courses
                          where c.Semester == semester
                          select new CourseDTO
                          {
                              ID           = c.ID,
                              Name         = "",
                              StartDate    = c.StartDate,
                              CourseID     = c.CourseID,
                              EndDate      = c.EndDate,
                              StudentCount = _db.CourseRegistrations.Count(x => x.CourseID == c.ID && x.Active == true)
                          }).ToList();

            return result;
        }

        /// <summary>
        /// Returns a single course with the specified ID
        /// If no course is found then a course not found exception is thrown
        /// </summary>
        /// <param name="courseModel"></param>
        /// <returns></returns>
        public CourseDetailsDTO GetCourseByID(int ID)
        {
            var course = _db.Courses.SingleOrDefault(x => x.ID == ID);

            // 1. Validation
            if(course == null)
            {
                throw new CourseNotFoundException();
            }

            var courseTemplate = _db.CourseTemplates.SingleOrDefault(x => x.CourseID == course.CourseID);
            if(courseTemplate == null)
            {
                throw new ApplicationException("Something went horribly wrong");
            }

            // Populating the list of students enrolled in the course
            var students = (from c in _db.CourseRegistrations
                            from p in _db.Persons
                            where c.CourseID == course.ID
                            where c.StudentID == p.ID
                            select new StudentDTO
                            {
                                ID   = c.StudentID,
                                Name = p.Name,
                                SSN  = p.SSN
                            }).ToList();

            var returnValue = new CourseDetailsDTO
            {
                ID        = course.ID,
                CourseID  = course.CourseID,
                Name      = courseTemplate.Name,
                StartDate = course.StartDate,
                EndDate   = course.EndDate,
                Students  = students
            };

            return returnValue;
        }

        /// <summary>
        /// Adds the course to the list of courses.
        /// If an entry for the given CourseID does not exist 
        /// in the course template list, an object not found exception is thrown.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDetailsDTO AddCourse(AddCourseViewModel course)
        {
            // 1. Validation
            var courseTemplate = _db.CourseTemplates.SingleOrDefault(x => x.CourseID == course.TemplateID);

            if (courseTemplate == null)
            {
                throw new CourseNotFoundException();
            }

            // 2. Create the database object
            var courseEntity = new Course
            {
                CourseID    = course.TemplateID,
                StartDate   = course.StartDate,
                EndDate     = course.EndDate,
                Semester    = course.Semester,
                MaxStudents = course.MaxStudents
            };

            _db.Courses.Add(courseEntity);
            _db.SaveChanges();

            // 3. Create the return 
            CourseDetailsDTO courseDTO = new CourseDetailsDTO
            {
                ID          = courseEntity.ID,
                CourseID    = courseEntity.CourseID,
                StartDate   = courseEntity.StartDate,
                EndDate     = courseEntity.EndDate,
                Name        = courseTemplate.Name,
                Students    = null,
                MaxStudents = courseEntity.MaxStudents
            };

            return courseDTO;
        }

        /// <summary>
        /// Updates the course with the given ID, if a course
        /// with the given ID was not found then null an object not 
        /// found exception is thrown.
        /// </summary>
        /// <returns></returns>
        public CourseDTO UpdateCourse(int ID, UpdateCourseViewModel course)
        {
            // 1. Validate


            // 2. Update
            var courseEntity = _db.Courses.SingleOrDefault(x => x.ID == ID);

            if(courseEntity == null)
            {
                throw new CourseNotFoundException();
            }

            courseEntity.StartDate   = course.StartDate;
            courseEntity.EndDate     = course.EndDate;
            courseEntity.MaxStudents = course.MaxStudents;

            _db.SaveChanges();

            // 3. Building the return value
            var courseTemplate = _db.CourseTemplates.SingleOrDefault(x => x.CourseID == courseEntity.CourseID);

            if (courseTemplate == null)
            {
                throw new ApplicationException("Something went horribly wrong");
            }

            var studentCount = _db.CourseRegistrations.Count(x => x.CourseID == courseEntity.ID && x.Active == true);

            var returnValue = new CourseDTO
            {
                ID           = courseEntity.ID,
                CourseID     = courseEntity.CourseID,
                StartDate    = courseEntity.StartDate,
                EndDate      = courseEntity.EndDate,
                Name         = courseTemplate.Name,
                StudentCount = studentCount
            };

            return returnValue;
        }

        /// <summary>
        /// Deletes the course with the given ID.
        /// If a course with the given ID was not found then
        /// an object not found exception is thrown.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public void DeleteCourse(int ID)
        {
            var course = _db.Courses.SingleOrDefault(x => x.ID == ID);

            if(course == null)
            {
                throw new CourseNotFoundException();
            }

            // Finding all course registrations for this course
            var courseRegistrations = (from c in _db.CourseRegistrations
                                       where c.CourseID == course.ID
                                       select c).ToList();

            // Removing all course registrations and the course itself
            foreach(var reg in courseRegistrations)
            {
                _db.CourseRegistrations.Remove(reg);
            }

            _db.Courses.Remove(course);
            _db.SaveChanges();
        }
        #endregion

        #region Students
        
        /// <summary>
        /// Returns a list of students signed up for the course
        /// with the given ID. If no students are signed up for the course
        /// an empty list should be returned.
        /// If no course is found for the given ID, en error is thrown.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<StudentDTO> GetStudentsByCourseID(int ID)
        {
            var course = _db.Courses.SingleOrDefault(x => x.ID == ID);

            // Validation
            if(course == null)
            {
                throw new CourseNotFoundException();
            }

            // Populating the list of students to return
            var students = (from r in _db.CourseRegistrations
                            from s in _db.Persons
                            where r.CourseID == course.ID
                            where r.StudentID == s.ID
                            where r.Active == true
                            select new StudentDTO
                            {
                                ID   = s.ID,
                                Name = s.Name,
                                SSN  = s.SSN
                            }).ToList();

            return students;
        }

        /// <summary>
        /// Adds the student to the course with the given ID
        /// If no course/student is found, an Object not found exception is thrown.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        public StudentDTO AddStudentToCourse(int ID, AddStudentToCourseViewModel student)
        {
            // 1. Validation
            var courseEntity = _db.Courses.FirstOrDefault(x => x.ID == ID);
            if(courseEntity == null)
            {
                throw new CourseNotFoundException();
            }

            var studentEntity = _db.Persons.FirstOrDefault(x => x.SSN == student.SSN);
            if (studentEntity == null)
            {
                throw new StudentNotFoundException();
            }

            // Checking to see if the course is full
            int studentCount = _db.CourseRegistrations.Count(x => x.CourseID == ID && x.Active == true);
            if (studentCount >= courseEntity.MaxStudents)
            {
                throw new CourseFullException();
            }

            // Checking to see if the student is already enrolled in the course.
            var enrolled = _db.CourseRegistrations.FirstOrDefault(x => x.StudentID == studentEntity.ID && x.CourseID == ID);
            if(enrolled != null)
            {
                if (enrolled.Active)
                {
                    throw new StudentAlreadyEnrolledException();
                }
            }

            // Checking to see if the student was on the waiting list
            // If he was, we remove him from it.
            var waiting = _db.CourseWaitingList.FirstOrDefault(x => x.StudentID == studentEntity.ID && x.CourseID == ID);
            if(waiting != null)
            {
                _db.CourseWaitingList.Remove(waiting);
                _db.SaveChanges();
            }

            // 2. Saving the registration
            // If the student was already enrolled and simply not active
            // We set his status to active.
            if(enrolled != null)
            {
                enrolled.Active = true;
            }
            else
            {
                var courseRegistration = new CourseRegistration
                {
                    CourseID = ID,
                    StudentID = studentEntity.ID,
                    Active = true
                };

                _db.CourseRegistrations.Add(courseRegistration);
                _db.SaveChanges();
            }
            
            // 3. Building the return object
            var studentDTO = new StudentDTO
            {
                ID = studentEntity.ID,
                Name = studentEntity.Name,
                SSN = studentEntity.SSN
            };

            return studentDTO;
        }

        /// <summary>
        /// Removes the student from the course with the given ID
        /// by setting his "active" status to false.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="studentSSN"></param>
        public void RemoveStudentFromCourse(int courseID, string studentSSN)
        {
            // 1. Validation
            var course = _db.Courses.SingleOrDefault(x => x.ID == courseID);

            if (course == null)
            {
                throw new CourseNotFoundException();
            }

            var student = _db.Persons.SingleOrDefault(x => x.SSN == studentSSN);
            if(student == null)
            {
                throw new StudentNotFoundException();
            }
            var registration = _db.CourseRegistrations.SingleOrDefault(x => x.StudentID == student.ID && x.CourseID == course.ID);

            // 2. Setting the entry to false
            registration.Active = false;
            _db.SaveChanges();
        }


        /// <summary>
        /// Returns a list of students on the waiting list for the 
        /// course with the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<StudentDTO> GetCourseWaitingList(int ID)
        {
            // 1. Validation
            var courseEntity = _db.Courses.FirstOrDefault(x => x.ID == ID);
            if (courseEntity == null)
            {
                throw new CourseNotFoundException();
            }

            // 2. Populating list of students
            // Populating the list of students to return
            var students = (from r in _db.CourseWaitingList
                            from s in _db.Persons
                            where r.CourseID == courseEntity.ID
                            where r.StudentID == s.ID
                            select new StudentDTO
                            {
                                ID = s.ID,
                                Name = s.Name,
                                SSN = s.SSN
                            }).ToList();


            return students;
        }

        /// <summary>
        /// Adds the student to the waiting list
        /// of the course with the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="student"></param>
        public StudentDTO AddStudentToWaitingList(int ID, AddStudentToCourseViewModel student)
        {
            // 1. Validation
            var courseEntity = _db.Courses.FirstOrDefault(x => x.ID == ID);
            if (courseEntity == null)
            {
                throw new CourseNotFoundException();
            }

            // Checking to see if the student exists
            var studentEntity = _db.Persons.FirstOrDefault(x => x.SSN == student.SSN);
            if (studentEntity == null)
            {
                throw new StudentNotFoundException();
            }

            // Checking to see if the student is already enrolled in the course.
            var enrolled = _db.CourseRegistrations.FirstOrDefault(x => x.StudentID == studentEntity.ID && x.CourseID == ID);
            if (enrolled != null)
            {
                if (enrolled.Active)
                {
                    throw new StudentAlreadyEnrolledException();
                }
            }

            // Checking to see if the student was already on the waiting list
            var waiting = _db.CourseWaitingList.FirstOrDefault(x => x.StudentID == studentEntity.ID && x.CourseID == ID);
            if (waiting != null)
            {
                throw new StudentAlreadyOnWaitingListException();
            }

            // 2. Saving the registration
            var courseRegistration = new CourseWaitingListRegistration
            {
                CourseID = ID,
                StudentID = studentEntity.ID
            };

            _db.CourseWaitingList.Add(courseRegistration);
            _db.SaveChanges();

            // 3. Building the return object
            var studentDTO = new StudentDTO
            {
                ID = studentEntity.ID,
                Name = studentEntity.Name,
                SSN = studentEntity.SSN
            };

            return studentDTO;
        }

        #endregion
    }
}
