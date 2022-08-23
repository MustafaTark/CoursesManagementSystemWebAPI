using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Branch { get; set; }
        public byte? Room { get; set; }
        [Column(TypeName = "money")]
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfDayes { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
     
        public ICollection<Student>? Students { get; set; }
        public Course()
        {
            this.Students=new HashSet<Student>();
        }

    }
}
