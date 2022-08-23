using CoursesManagementSystem.Data.Configration;
using CoursesManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CoursesManagementSystem.Data
{
    public class ApplicationDb:IdentityDbContext<User>
    {
        public DbSet<Student>? Students { get; set; }
        public DbSet<Course>? Courses { get; set; }
        public DbSet<Instructor>? Instructors { get; set; }
        public DbSet<StudentGrade>? StudentGrades { get; set; }
        public DbSet<InstructorPost>? InstructorPosts { get; set; }
        public ApplicationDb(DbContextOptions<ApplicationDb> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Course>()
               .HasMany(s => s.Students)
               .WithMany(c => c.Courses);
            builder.Entity<StudentGrade>().HasOne(sg => sg.Course);

            builder.ApplyConfiguration(new RoleConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
