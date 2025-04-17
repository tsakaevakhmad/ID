using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ID.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUsernameFiledFromFidoCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "FidoCredentials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "FidoCredentials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
