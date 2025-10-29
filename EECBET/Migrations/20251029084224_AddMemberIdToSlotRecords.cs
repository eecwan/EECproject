using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EECBET.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberIdToSlotRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "SlotRecords",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "SlotRecords");
        }
    }
}
