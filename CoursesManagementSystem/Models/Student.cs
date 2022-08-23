using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesManagementSystem.Models
{
    public class Student 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
     
        [StringLength(12)]
        public string? Phone { get; set; }
      
        public ICollection<StudentGrade>? Grades { get; set; }
       
        public bool IsPayed { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public Student()
        {
            Courses = new HashSet<Course>();
            Grades = new HashSet<StudentGrade>();
        }

    }
}
