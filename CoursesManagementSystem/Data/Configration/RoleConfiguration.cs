using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CoursesManagementSystem.Data.Configration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        { 
            builder.HasData(
                new IdentityRole { Name = "CourseAdmin", NormalizedName = "COURSEADMIN" },
                new IdentityRole { Name = "Instructor", NormalizedName = "INSTRUCTOR" },
                new IdentityRole { Name = "Student", NormalizedName = "STUDENT" }
                );
        }
    }
}
