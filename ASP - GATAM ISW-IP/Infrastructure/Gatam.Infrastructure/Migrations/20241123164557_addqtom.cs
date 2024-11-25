using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gatam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addqtom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Modules_ApplicationModuleId",
                table: "Questions");

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

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Category", "CreatedAt", "Title" },
                values: new object[] { "cf91760c-0153-4222-9e97-94f562e9f619", "SollicitatieTraining", new DateTime(2024, 11, 23, 16, 45, 56, 591, DateTimeKind.Utc).AddTicks(4515), "Solliciteren voor beginners" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "ApplicationModuleId", "CreatedAt", "CreatedUserId", "LastUpdatedAt", "LastUpdatedUserId", "QuestionTitle", "QuestionType" },
                values: new object[] { "31a796ad-84fa-4159-b683-e6cc4c53142a", null, new DateTime(2024, 11, 23, 16, 45, 56, 592, DateTimeKind.Utc).AddTicks(2040), "123", new DateTime(2024, 11, 23, 16, 45, 56, 592, DateTimeKind.Utc).AddTicks(2042), "123", "Wat wil je later bereiken? ", (short)1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BegeleiderId", "Email", "IsActive", "Name", "PasswordHash", "PhoneNumber", "Picture", "RolesIds", "Surname", "Username" },
                values: new object[,]
                {
                    { "2e907a03-1af2-4b23-9fab-6a184bbd776c", null, "jane.doe@example.com", false, "JaneDoe", "AQAAAAIAAYagAAAAENRvzPJyHB5eD9NKp6W4s6bLlIm/Q0Mkm59+j/RfUz84Y7V6WX/S3IVD9PvFjUuqPA==", "+32 568779633", "png", "[\"rol_tj8keS38380ZU4NR\"]", "JANEDOE", "JaneDoeJANEDOE" },
                    { "502cb1a5-f321-4dba-8ee7-224f955d6e66", null, "john.doe@example.com", false, "JohnDoe", "AQAAAAIAAYagAAAAEONNkSH8zIbcPZPkbeTqZjTQovNY2yvvF+sl6oYr9enaHeADGQ8w3OpiUAL0VxVQjg==", "+32 456789166", "png", "[\"rol_2SgoYL1AoK9tXYXW\"]", "JOHNDOE", "JohnDoeJOHNDOE" },
                    { "96b82fa0-f8b4-4bbb-908d-345580008078", null, "admin@app.com", false, "admin", "AQAAAAIAAYagAAAAELzFIAUEjrRBCRR0uX3SDFLt7VFxT7vVw9Wziu5iog3Gnrf3LDZMqYcAoyUULR2log==", "+32 9966554411", "png", "[\"rol_3BsJHRFokYkbjr5O\"]", "Suradmin", "adminSuradmin" },
                    { "db869a62-e9a8-454c-ae7d-9dd4a313f492", null, "lautje.doe@example.com", false, "Lautje", "AQAAAAIAAYagAAAAEJMklvEYOOXWKvyNwK8Zq6r/AUDIgNeOr/Rgw0Ljv2NjrwOrBa3EcgsBGIx4DLXWmQ==", "+23 7896544336", "png", "[\"rol_tj8keS38380ZU4NR\"]", "LAUTJE", "LautjeLAUTJE" }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Answer", "AnswerValue", "QuestionId" },
                values: new object[] { "1e042bcc-0e5a-463a-af54-2ed62067429c", "OPEN", null, "31a796ad-84fa-4159-b683-e6cc4c53142a" });

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Modules_ApplicationModuleId",
                table: "Questions",
                column: "ApplicationModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Modules_ApplicationModuleId",
                table: "Questions");

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: "1e042bcc-0e5a-463a-af54-2ed62067429c");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: "cf91760c-0153-4222-9e97-94f562e9f619");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2e907a03-1af2-4b23-9fab-6a184bbd776c");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "502cb1a5-f321-4dba-8ee7-224f955d6e66");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "96b82fa0-f8b4-4bbb-908d-345580008078");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "db869a62-e9a8-454c-ae7d-9dd4a313f492");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: "31a796ad-84fa-4159-b683-e6cc4c53142a");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Modules_ApplicationModuleId",
                table: "Questions",
                column: "ApplicationModuleId",
                principalTable: "Modules",
                principalColumn: "Id");
        }
    }
}
