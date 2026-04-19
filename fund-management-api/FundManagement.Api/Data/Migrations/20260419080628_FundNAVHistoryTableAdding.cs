using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FundNAVHistoryTableAdding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FundNAVHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundId = table.Column<int>(type: "int", nullable: false),
                    NAV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundNAVHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundNAVHistories_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundNAVHistories_FundId_Date",
                table: "FundNAVHistories",
                columns: new[] { "FundId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundNAVHistories");
        }
    }
}
