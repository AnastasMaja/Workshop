namespace Workshop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public long Id { get; set; }

        [Required]
        [StringLength(10)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        public int? AcquiredCredits { get; set; }
        public int? CurrentSemestar { get; set; }

        [StringLength(25)]
        public string? EducationLevel { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName} ({StudentId})"; }
        }
    }
}
