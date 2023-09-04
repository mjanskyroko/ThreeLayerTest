using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWebApp.Infrastructure.Database.Mssql.Migrations
{
    /// <inheritdoc />
    public partial class AccountActiveField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Accounts");
        }
    }
}
