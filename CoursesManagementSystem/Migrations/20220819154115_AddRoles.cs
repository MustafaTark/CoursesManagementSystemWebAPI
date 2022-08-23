using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesManagementSystem.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81116ae7-638c-4294-b62f-769616f4f4e1", "c3c3c29b-6756-44d9-9500-9b2d18281b7a", "Instructor", "INSTRUCTOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "89eb6d03-d5fe-432d-9cb2-1991b6cb9b21", "55ad02b6-b49a-44b1-af42-49f95b43fc6b", "CourseAdmin", "COURSEADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ff94ce6a-4c2a-4813-b7a7-14a206b3c6b2", "ceff52b3-8441-4533-a318-0ab57192ea51", "Student", "STUDENT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81116ae7-638c-4294-b62f-769616f4f4e1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89eb6d03-d5fe-432d-9cb2-1991b6cb9b21");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff94ce6a-4c2a-4813-b7a7-14a206b3c6b2");
        }
    }
}
