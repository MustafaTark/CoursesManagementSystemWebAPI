using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesManagementSystem.Migrations
{
    public partial class NewGrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentGrades_StudentGradeId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "StudentGrades");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGrades_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students",
                column: "StudentGradeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_CourseId",
                table: "StudentGrades",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentGrades_StudentGradeId",
                table: "Students",
                column: "StudentGradeId",
                principalTable: "StudentGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
