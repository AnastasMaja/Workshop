
namespace Workshop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Degree { get; set; }

        [StringLength(25)]
        public string AcademicRank { get; set; }

        [StringLength(10)]
        public string OfficeNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public ICollection<Course> CoursesAsFirstTeacher { get; set; }
        public ICollection<Course> CoursesAsSecondTeacher { get; set; }
        public string FullName
        {
            get { return $"{FirstName} {LastName} ({AcademicRank})"; }
        }
    }

   
}
