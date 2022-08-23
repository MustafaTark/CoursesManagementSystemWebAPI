using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesManagementSystem.Migrations
{
    public partial class FinishGrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentGrades_StudentGradeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentGradeId",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_StudentId",
                table: "StudentGrades",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_Students_StudentId",
                table: "StudentGrades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Students_StudentId",
                table: "StudentGrades");

            migrationBuilder.DropIndex(
                name: "IX_StudentGrades_StudentId",
                table: "StudentGrades");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentGrades");

            migrationBuilder.AddColumn<int>(
                name: "StudentGradeId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentGradeId",
                table: "Students",
                column: "StudentGradeId");

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
