using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingnamefromUserCourseEntitytoUserCourseasmytablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourseEntity_AspNetUsers_UserId",
                table: "UserCourseEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourseEntity_Courses_CourseId",
                table: "UserCourseEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourseEntity",
                table: "UserCourseEntity");

            migrationBuilder.RenameTable(
                name: "UserCourseEntity",
                newName: "UserCourse");

            migrationBuilder.RenameIndex(
                name: "IX_UserCourseEntity_CourseId",
                table: "UserCourse",
                newName: "IX_UserCourse_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_AspNetUsers_UserId",
                table: "UserCourse",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_Courses_CourseId",
                table: "UserCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_AspNetUsers_UserId",
                table: "UserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_Courses_CourseId",
                table: "UserCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse");

            migrationBuilder.RenameTable(
                name: "UserCourse",
                newName: "UserCourseEntity");

            migrationBuilder.RenameIndex(
                name: "IX_UserCourse_CourseId",
                table: "UserCourseEntity",
                newName: "IX_UserCourseEntity_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourseEntity",
                table: "UserCourseEntity",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourseEntity_AspNetUsers_UserId",
                table: "UserCourseEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourseEntity_Courses_CourseId",
                table: "UserCourseEntity",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
