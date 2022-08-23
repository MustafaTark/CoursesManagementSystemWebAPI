using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesManagementSystem.Models
{
    public class StudentGrade
    {
        public int Id { get; set; }
        public double Grade { get; set; }
        public Student? Student { get; set; }
        public int StudentId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

    }
}
