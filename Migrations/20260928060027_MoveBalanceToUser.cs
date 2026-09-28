using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAccountApi.Migrations
{
    /// <inheritdoc />
    public partial class MoveBalanceToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BankAccountItems");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Users",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "Balance",
                table: "BankAccountItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
