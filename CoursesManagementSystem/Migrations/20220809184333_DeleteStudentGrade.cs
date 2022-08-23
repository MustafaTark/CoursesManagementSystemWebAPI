using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesManagementSystem.Migrations
{
    public partial class DeleteStudentGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentGrades");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students",
                column: "StudentGradeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students",
                column: "StudentGradeId",
                unique: true);
        }
    }
}
