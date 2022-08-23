using System.ComponentModel.DataAnnotations;

namespace CoursesManagementSystem.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
       
        [StringLength(12)]
        public string? Phone { get; set; }
        public ICollection<InstructorPost>? Posts { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public Instructor()
        {
            this.Posts = new HashSet<InstructorPost>();
            this.Courses = new HashSet<Course>();
        }
    }
}
