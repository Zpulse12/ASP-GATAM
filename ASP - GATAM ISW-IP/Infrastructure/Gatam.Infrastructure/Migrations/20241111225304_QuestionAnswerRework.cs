using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gatam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionAnswerRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "05010e2f-ffb3-4e6a-9e52-1a185f5620a2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2dc041c2-34f7-4328-8e07-ad782a3e5b88");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5f1ff2bb-dc4a-46a3-88e8-987e07ef7474");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6fd0100a-f0d1-4165-bee8-ed85192ef49f");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: "431fe43d-81bd-4580-83db-f0d5082decb5");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "05010e2f-ffb3-4e6a-9e52-1a185f5620a2", 0, "843107fa-f8c6-43ce-a2d2-ea642f09ad75", "john.doe@example.com", false, false, false, null, "JOHN.DOE@EXAMPLE.COM", "JOHNDOE", "AQAAAAIAAYagAAAAEFgZO8lLH/kGutwV9x/MZMZF9xL8PXf8JPIE2sAECoA4AkauO47A5rMrPQ7CORycWA==", null, false, "bcd30cb3-9bb4-4fed-b007-351ed59a3bb7", false, "JohnDoe" },
                    { "2dc041c2-34f7-4328-8e07-ad782a3e5b88", 0, "ee1f8fe2-5afc-414e-813c-c869c3b68149", "lautje.doe@example.com", false, false, false, null, "LAUTJE.DOE@EXAMPLE.COM", "LAUTJE", "AQAAAAIAAYagAAAAEG2pXyAs7wXn/TcKOTwzrfncVvQUMfXMXwoWc7K7y1ksLVUDrE3JMdoRlbSLGi8t7A==", null, false, "d79259b4-8b3c-428e-8cac-c558b930384d", false, "Lautje" },
                    { "5f1ff2bb-dc4a-46a3-88e8-987e07ef7474", 0, "d21d056f-7ceb-46a4-88ff-b4a2f9ce532f", "jane.doe@example.com", false, false, false, null, "JANE.DOE@EXAMPLE.COM", "JANEDOE", "AQAAAAIAAYagAAAAEPBBRdE4xeY6Z6khvteCJ0HZMsb58XN6tM1dJyVCJkQbCx8xVQ8vV+hMHLKXBw03jg==", null, false, "7f1321ff-c337-4566-bbf2-6773660b3139", false, "JaneDoe" },
                    { "6fd0100a-f0d1-4165-bee8-ed85192ef49f", 0, "62a721c2-70cb-4d96-8ad4-2d988a580ed6", "admin@app.com", false, false, false, null, null, null, "AQAAAAIAAYagAAAAELBV+L6Ybt0FyBOx8ve3W87i6SFFAmX4h7s2tnoqcnRNrj2VoWe+KdO1Zm0/YGf7qQ==", null, false, "ba60aef3-bb45-4253-a1a7-3191871ad04c", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Category", "CreatedAt", "Title" },
                values: new object[] { "431fe43d-81bd-4580-83db-f0d5082decb5", "SollicitatieTraining", new DateTime(2024, 11, 11, 22, 38, 51, 461, DateTimeKind.Utc).AddTicks(9135), "Solliciteren voor beginners" });
        }
    }
}
