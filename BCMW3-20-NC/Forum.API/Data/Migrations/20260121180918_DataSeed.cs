using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CommentsAreAllowed", "Content", "CreateDate", "ImageUrl", "LastCommentDate", "Title" },
                values: new object[,]
                {
                    { new Guid("1cc11272-2f02-4bca-a0f8-abd32f970a32"), true, "Hello World!", new DateTime(2026, 1, 21, 22, 9, 17, 468, DateTimeKind.Local).AddTicks(9664), null, new DateTime(2026, 1, 21, 22, 9, 17, 468, DateTimeKind.Local).AddTicks(9664), "Second Topic" },
                    { new Guid("7d5f3b62-71dc-4683-8665-5342b8a17975"), true, "Hello World!", new DateTime(2026, 1, 21, 22, 9, 17, 468, DateTimeKind.Local).AddTicks(8934), null, new DateTime(2026, 1, 21, 22, 9, 17, 468, DateTimeKind.Local).AddTicks(9371), "First Topic" },
                    { new Guid("e80d9e8d-4fa3-4a55-8bc7-b51fd8542f18"), true, "Hello World!", new DateTime(2026, 1, 21, 22, 9, 17, 468, DateTimeKind.Local).AddTicks(9661), null, new DateTime(2026, 1, 21, 22, 9, 17, 468, DateTimeKind.Local).AddTicks(9662), "Second Topic" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "CommentDate", "Content", "TopicId" },
                values: new object[,]
                {
                    { new Guid("46c36068-13ab-47a3-a1e5-e74e07d888cb"), new DateTime(2026, 1, 23, 22, 9, 17, 469, DateTimeKind.Local).AddTicks(5631), "Hello Comment!", new Guid("e80d9e8d-4fa3-4a55-8bc7-b51fd8542f18") },
                    { new Guid("dae65b74-fd79-47ab-aa78-df29cae7351b"), new DateTime(2026, 1, 22, 22, 9, 17, 469, DateTimeKind.Local).AddTicks(5052), "Hello Comment!", new Guid("7d5f3b62-71dc-4683-8665-5342b8a17975") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("46c36068-13ab-47a3-a1e5-e74e07d888cb"));

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("dae65b74-fd79-47ab-aa78-df29cae7351b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1cc11272-2f02-4bca-a0f8-abd32f970a32"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7d5f3b62-71dc-4683-8665-5342b8a17975"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e80d9e8d-4fa3-4a55-8bc7-b51fd8542f18"));
        }
    }
}
