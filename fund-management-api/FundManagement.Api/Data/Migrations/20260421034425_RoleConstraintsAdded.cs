using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RoleConstraintsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Role",
                table: "Users",
                sql: "[Role] IN ('Admin', 'Investor')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Role",
                table: "Users");
        }
    }
}
