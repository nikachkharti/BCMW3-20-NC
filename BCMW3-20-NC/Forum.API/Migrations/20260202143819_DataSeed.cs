using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "33B7ED72-9434-434A-82D4-3018B018CB87", "db9f3b0f-9019-4fc0-8207-6116661f147a", "Admin", "ADMIN" },
                    { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", "68c1bec4-42df-4875-9daa-9023015f8dc8", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "D2A3C6B1-5F4E-4F2A-9C3D-1E2F3A4B5C6D", 0, "e4bb316d-1c3f-485a-9693-cd1aff1d2413", "admin@gmail.com", false, true, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEJ3りんLw8+FvqR5z0t5K8xH/0GQbQZJ3dP2vN8kM1xQ7vJ3zX9rN2wK5fL8pY6mA==", "555337681", false, "a888687f-c264-4ead-99d8-6bf8c92e3032", false, "admin@gmail.com" },
                    { "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E", 0, "8f12db43-afad-449d-af24-1adc4ef52e4b", "nika@gmail.com", false, true, null, "NIKA@GMAIL.COM", "NIKA@GMAIL.COM", "AQAAAAIAAYagAAAAEBvN8mK2xL9+TwpQ6r1u4T7Y/1FRcRaK4eQ3wO9lN2yR8wK6aM4{Y0sO3xM6gO7nB==", "558490645", false, "a09bcdc8-14fb-48ef-a0d7-b9d84a5ca6f2", false, "nika@gmail.com" },
                    { "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F", 0, "b9a93707-f331-4b4e-b4b3-f0870abbc869", "gio@gmail.com", false, true, null, "GIO@GMAIL.COM", "GIO@GMAIL.COM", "AQAAAAIAAYagAAAAECwP9nL3yM0+UxqR7s2v5U8Z/2GSdScL5fR4xP0mO3zS9xL7bN5aZ1tP4yN7hP8oC==", "551442269", false, "5d09c2d3-b5cb-4354-84b3-640a855d07a1", false, "gio@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "AuthorId", "CommentsAreAllowed", "Content", "CreateDate", "ImageUrl", "LastCommentDate", "Title" },
                values: new object[,]
                {
                    { "4B363C0A-6917-46FA-AAC0-815F2FD9351B", "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F", true, "Hello World!", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Second Topic" },
                    { "9EC8A152-A054-4DE9-B30A-C9286E03ED72", "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E", true, "Hello World!", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "First Topic" },
                    { "F9B01BD8-C9DB-4DA9-A1CD-46606EB0DADF", "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F", true, "Hello World!", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Third Topic" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "33B7ED72-9434-434A-82D4-3018B018CB87", "D2A3C6B1-5F4E-4F2A-9C3D-1E2F3A4B5C6D" },
                    { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E" },
                    { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "CommentDate", "Content", "TopicId" },
                values: new object[,]
                {
                    { "72775F5B-0878-483E-97EA-2BC255AA8B74", "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Hello Comment!", "4B363C0A-6917-46FA-AAC0-815F2FD9351B" },
                    { "A59E8CEF-D838-4E7C-8396-86B1B3A96A98", "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Hello Comment!", "9EC8A152-A054-4DE9-B30A-C9286E03ED72" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: "72775F5B-0878-483E-97EA-2BC255AA8B74");

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: "A59E8CEF-D838-4E7C-8396-86B1B3A96A98");

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: "F9B01BD8-C9DB-4DA9-A1CD-46606EB0DADF");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "33B7ED72-9434-434A-82D4-3018B018CB87", "D2A3C6B1-5F4E-4F2A-9C3D-1E2F3A4B5C6D" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "33B7ED72-9434-434A-82D4-3018B018CB87");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B");

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: "4B363C0A-6917-46FA-AAC0-815F2FD9351B");

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: "9EC8A152-A054-4DE9-B30A-C9286E03ED72");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "D2A3C6B1-5F4E-4F2A-9C3D-1E2F3A4B5C6D");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F");
        }
    }
}
