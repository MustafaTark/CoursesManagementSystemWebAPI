using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesManagementSystem.Models
{
    public class InstructorPost
    {
       public int Id { get; set; }
        [Column(TypeName = "ntext")]
        public string? Content { get; set; }
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
       

    }
}
