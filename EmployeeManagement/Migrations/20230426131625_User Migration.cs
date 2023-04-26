using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class UserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Employee",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_IdentityUserId",
                table: "Employee",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_AspNetUsers_IdentityUserId",
                table: "Employee",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_AspNetUsers_IdentityUserId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_IdentityUserId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employee");
        }
    }
}
