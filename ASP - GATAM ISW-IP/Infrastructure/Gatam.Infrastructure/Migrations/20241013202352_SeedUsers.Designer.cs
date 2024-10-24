using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gatam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsersDesigner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash", "SecurityStamp" },
                values: new object[] { "312234d9-f18b-4bc9-9b30-05d64eb2c3ba", false, "AQAAAAIAAYagAAAAEDbEUf8Tnk+MrKMFv3u84z9QVaJTlzJYa98AtSC6jHUS5ke/kGnAi2An5ywehyN1ug==", "34222c33-d874-41c6-8e26-8d32a52b68ce" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0bb2dfb9-411e-477b-8163-3917fa8f1262", false, "AQAAAAIAAYagAAAAELhUCLOoYNTkVVftoUrQuU9nh5sp1BX3OZonSG2gzsJ9OGHEOp5dIcfRYpK5NaIFpA==", "70d3db0f-b719-4a6b-9621-e00666b9bf93" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "63b93d36-6394-4079-be94-4096bfff3e04", "AQAAAAIAAYagAAAAEKs6k3odID6YVDh8kv5g5EBT8pgVodqs5yykymppEoBHfpJbM9wyM06hpitEkD27wA==", "d3afe8e3-ff4e-4726-aaa2-b04b7b4823d6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e40a3a64-bbc2-4fa3-ad95-22bfc2f33828", "AQAAAAIAAYagAAAAEOpmwFQOj7WKJv7/u27sfySWie4YnPD/VO5IoX8CjJKHp3oBdePMDZys4NRe1h6zHQ==", "bc62b443-9460-4cf0-8052-798b69701dd8" });
        }
    }
}
