using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gatam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class questionToModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: "f3f5de95-83bd-41a0-9575-789201656922");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "030cb83f-5ee6-4665-aaaf-1ed9e624c216");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "38083661-96b8-4972-94da-434578855c2f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ac3b8446-f3bd-476e-8972-43a783c2c530");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ade10a19-81ce-4f9b-b0af-2cf1e323823a");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: "b8f7182a-c1f8-4764-b8be-d53470a93222");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "5f78127c-d05c-4265-a142-337dd2aca768");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "Users",
                newName: "RolesIds");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "Users",
                newName: "Picture");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BegeleiderId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserModule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModule_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModule_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Category", "CreatedAt", "Title" },
                values: new object[] { "b8bcbe58-226e-4142-a8e5-2abfc182cccf", "SollicitatieTraining", new DateTime(2024, 11, 23, 10, 24, 23, 329, DateTimeKind.Utc).AddTicks(2212), "Solliciteren voor beginners" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "ApplicationModuleId", "CreatedAt", "CreatedUserId", "LastUpdatedAt", "LastUpdatedUserId", "QuestionTitle", "QuestionType" },
                values: new object[] { "d4d48c91-3391-4822-819d-bad61e255c3b", null, new DateTime(2024, 11, 23, 10, 24, 23, 330, DateTimeKind.Utc).AddTicks(556), "123", new DateTime(2024, 11, 23, 10, 24, 23, 330, DateTimeKind.Utc).AddTicks(558), "123", "Wat wil je later bereiken? ", (short)1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BegeleiderId", "Email", "IsActive", "Name", "PasswordHash", "PhoneNumber", "Picture", "RolesIds", "Surname", "Username" },
                values: new object[,]
                {
                    { "35e8cd4b-2209-47e6-870b-526e5e177976", null, "admin@app.com", false, "admin", "AQAAAAIAAYagAAAAEN0C+lKCQ/iOxeEqGM7IjLiGKkOTZJf7asc+H1CViUWJXQ+O9cFdVYOuP/hiqETGnA==", "+32 9966554411", "png", "[\"rol_3BsJHRFokYkbjr5O\"]", "Suradmin", "adminSuradmin" },
                    { "5a489be0-d5c9-4eb0-a0cc-67876bba6506", null, "lautje.doe@example.com", false, "Lautje", "AQAAAAIAAYagAAAAEHUT+nCz7rTfc5+k2ZvfrekZKm0xyque/4VbLh4j2bQSWtFnE92jzdAR9KEjlBZC2Q==", "+23 7896544336", "png", "[\"rol_tj8keS38380ZU4NR\"]", "LAUTJE", "LautjeLAUTJE" },
                    { "ac90648f-b8f9-4b18-a671-3bb9c081f703", null, "john.doe@example.com", false, "JohnDoe", "AQAAAAIAAYagAAAAEInst+LraTcuHn9qQSAphwXYEyU9UADquoo5VvzLJJByKESaKYQ7U7ij77DHkdv5rw==", "+32 456789166", "png", "[\"rol_2SgoYL1AoK9tXYXW\"]", "JOHNDOE", "JohnDoeJOHNDOE" },
                    { "f06815b1-1295-4e77-bf13-beb124cc49af", null, "jane.doe@example.com", false, "JaneDoe", "AQAAAAIAAYagAAAAEHjxh0VgpsUuLjSIY1vFyB26q8l/O1e6rzubS9Q9Np+GX0QtDoSejYwybkZ1JVpEDQ==", "+32 568779633", "png", "[\"rol_tj8keS38380ZU4NR\"]", "JANEDOE", "JaneDoeJANEDOE" }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Answer", "AnswerValue", "QuestionId" },
                values: new object[] { "dbbc39a6-3326-4ebe-b608-592adac87c06", "OPEN", null, "d4d48c91-3391-4822-819d-bad61e255c3b" });

            migrationBuilder.CreateIndex(
                name: "IX_UserModule_ModuleId",
                table: "UserModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModule_UserId",
                table: "UserModule",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: "dbbc39a6-3326-4ebe-b608-592adac87c06");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: "b8bcbe58-226e-4142-a8e5-2abfc182cccf");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "35e8cd4b-2209-47e6-870b-526e5e177976");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a489be0-d5c9-4eb0-a0cc-67876bba6506");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "ac90648f-b8f9-4b18-a671-3bb9c081f703");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "f06815b1-1295-4e77-bf13-beb124cc49af");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "d4d48c91-3391-4822-819d-bad61e255c3b");

            migrationBuilder.DropColumn(
                name: "BegeleiderId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "RolesIds",
                table: "AspNetUsers",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "AspNetUsers",
                newName: "ConcurrencyStamp");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "030cb83f-5ee6-4665-aaaf-1ed9e624c216", 0, "67c31f29-a20d-4959-b53f-c25d40d4ab25", "lautje.doe@example.com", false, false, false, null, "LAUTJE.DOE@EXAMPLE.COM", "LAUTJE", "AQAAAAIAAYagAAAAEB+ngvCZPcbMaXuxCV43VW55aAX1Mb0xK7n+jeylEQLWBt/blOc6svn39XdpzOIH6A==", null, false, "1879364f-03f8-4325-9403-1739e62f3fd7", false, "Lautje" },
                    { "38083661-96b8-4972-94da-434578855c2f", 0, "76e73f55-264c-44c0-96a1-a65b188e7fcd", "admin@app.com", false, false, false, null, null, null, "AQAAAAIAAYagAAAAEDQVe8UFAl6hiyK/rrR4m3mQscQx0t5nRDU70/gxLnI/qyyw/L+Z3ZtdBWDRIZBmzg==", null, false, "33e9aebb-c8cb-447a-8ebc-90c18ca150bc", false, "admin" },
                    { "ac3b8446-f3bd-476e-8972-43a783c2c530", 0, "192eb663-5a1b-405b-b5c2-586dd6956b9a", "jane.doe@example.com", false, false, false, null, "JANE.DOE@EXAMPLE.COM", "JANEDOE", "AQAAAAIAAYagAAAAEMiNrOA2jPosldjXzfVCyFZ35x8pclnn6+LCjUE73Y0USLy3YI+UtZ3OK/45n/rGfw==", null, false, "355cbbbf-c320-4ea3-8275-c70881810afa", false, "JaneDoe" },
                    { "ade10a19-81ce-4f9b-b0af-2cf1e323823a", 0, "92951a47-5137-441a-8644-d8eda61c6705", "john.doe@example.com", false, false, false, null, "JOHN.DOE@EXAMPLE.COM", "JOHNDOE", "AQAAAAIAAYagAAAAEHsKSBOE6BS9WbiC2o3aihbvTPdVQ9FxIlP2zA1zTDTHFxy4qWF001MGW2VghX8EGA==", null, false, "172f3bfa-4926-4b8e-a60b-956e238debe1", false, "JohnDoe" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Category", "CreatedAt", "Title" },
                values: new object[] { "b8f7182a-c1f8-4764-b8be-d53470a93222", "SollicitatieTraining", new DateTime(2024, 11, 11, 22, 53, 4, 263, DateTimeKind.Utc).AddTicks(1593), "Solliciteren voor beginners" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "ApplicationModuleId", "CreatedAt", "CreatedUserId", "LastUpdatedAt", "LastUpdatedUserId", "QuestionTitle", "QuestionType" },
                values: new object[] { "5f78127c-d05c-4265-a142-337dd2aca768", null, new DateTime(2024, 11, 11, 22, 53, 4, 263, DateTimeKind.Utc).AddTicks(1646), "123", new DateTime(2024, 11, 11, 22, 53, 4, 263, DateTimeKind.Utc).AddTicks(1647), "123", "Wat wil je later bereiken? ", (short)1 });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Answer", "AnswerValue", "QuestionId" },
                values: new object[] { "f3f5de95-83bd-41a0-9575-789201656922", "OPEN", null, "5f78127c-d05c-4265-a142-337dd2aca768" });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");
        }
    }
}
