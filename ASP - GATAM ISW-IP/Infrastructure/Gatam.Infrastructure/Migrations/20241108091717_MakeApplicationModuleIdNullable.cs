using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gatam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeApplicationModuleIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DropColumn(
                name: "_role",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionType = table.Column<short>(type: "smallint", nullable: false),
                    QuestionTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationModuleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QuestionAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Modules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "3bf7572d-0861-416c-a970-caed2364a3a8", 0, "fa51408c-48fb-48f3-b65e-67e686f1c21f", "admin@app.com", false, false, false, null, null, null, "AQAAAAIAAYagAAAAEM+ySfnTMgfJ/moaq7Tzj567rM+ej6nyPsO4Pf0UOdNmuBjoZRCl5RqpAn4NQGJcMQ==", null, false, "0abd56f5-073f-4d88-ac1d-bf2c0b5d3a9a", false, "admin" },
                    { "8b53a729-93a9-4c43-816e-838f4b9f9634", 0, "46b75a92-57fd-48f0-9970-f33b21fbffe6", "lautje.doe@example.com", false, false, false, null, "LAUTJE.DOE@EXAMPLE.COM", "LAUTJE", "AQAAAAIAAYagAAAAENAyuk1FRIQIOT2yFUs1OsHUdyMOe1P22glLNkAPUEaKrHWoWGruvvDB+eboPZW4mA==", null, false, "bbd2b256-ed6f-4022-ba15-152d2bf2b95c", false, "Lautje" },
                    { "daead2ba-b127-49c1-aca4-fd3ad8ed47dc", 0, "7ef2b17d-7676-4e97-aafb-8ba582744e61", "john.doe@example.com", false, false, false, null, "JOHN.DOE@EXAMPLE.COM", "JOHNDOE", "AQAAAAIAAYagAAAAEJ2xGNe7cc7Km6Ivcnpjf+FBRNJga7sjGYQZdabpp5LaEhAOiavKBVp4n4nlo3Ic7w==", null, false, "459b4605-b065-420d-b212-ada4ddacb377", false, "JohnDoe" },
                    { "e678e6aa-dd5f-4d41-81cf-40f7784e748e", 0, "8a2a3441-70aa-4e03-86ad-d2141b6aae49", "jane.doe@example.com", false, false, false, null, "JANE.DOE@EXAMPLE.COM", "JANEDOE", "AQAAAAIAAYagAAAAEIoxpcjMsSgWol2ZOdWLggxNLvsm3z2Oiy6idazMMACdKja2VcZE2PAeTTLYHU82gg==", null, false, "c2f0c340-0ce9-4a5c-9e38-4805de55c229", false, "JaneDoe" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Category", "CreatedAt", "Title" },
                values: new object[] { "d615fa20-3ffd-49b9-8715-3fb5fafa158d", "SollicitatieTraining", new DateTime(2024, 11, 8, 9, 17, 16, 414, DateTimeKind.Utc).AddTicks(3241), "Solliciteren voor beginners" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "ApplicationModuleId", "CreatedAt", "CreatedUserId", "LastUpdatedAt", "LastUpdatedUserId", "QuestionAnswer", "QuestionTitle", "QuestionType" },
                values: new object[] { "3fe98c97-3951-4aca-b3cd-521f9679b987", null, new DateTime(2024, 11, 8, 9, 17, 16, 414, DateTimeKind.Utc).AddTicks(3292), "123", new DateTime(2024, 11, 8, 9, 17, 16, 414, DateTimeKind.Utc).AddTicks(3294), "123", "OPEN", "Wat wil je later bereiken? ", (short)1 });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ApplicationModuleId",
                table: "Questions",
                column: "ApplicationModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3bf7572d-0861-416c-a970-caed2364a3a8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8b53a729-93a9-4c43-816e-838f4b9f9634");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "daead2ba-b127-49c1-aca4-fd3ad8ed47dc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e678e6aa-dd5f-4d41-81cf-40f7784e748e");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "_role",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "_role" },
                values: new object[,]
                {
                    { "1", 0, "63b93d36-6394-4079-be94-4096bfff3e04", "john.doe@example.com", false, false, null, "JOHN.DOE@EXAMPLE.COM", "JOHNDOE", "AQAAAAIAAYagAAAAEKs6k3odID6YVDh8kv5g5EBT8pgVodqs5yykymppEoBHfpJbM9wyM06hpitEkD27wA==", null, false, "d3afe8e3-ff4e-4726-aaa2-b04b7b4823d6", false, "JohnDoe", 0 },
                    { "2", 0, "e40a3a64-bbc2-4fa3-ad95-22bfc2f33828", "jane.doe@example.com", false, false, null, "JANE.DOE@EXAMPLE.COM", "JANEDOE", "AQAAAAIAAYagAAAAEOpmwFQOj7WKJv7/u27sfySWie4YnPD/VO5IoX8CjJKHp3oBdePMDZys4NRe1h6zHQ==", null, false, "bc62b443-9460-4cf0-8052-798b69701dd8", false, "JaneDoe", 0 }
                });
        }
    }
}
