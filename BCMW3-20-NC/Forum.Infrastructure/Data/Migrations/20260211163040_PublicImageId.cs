using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PublicImageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastCommentDate",
                table: "Topics",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AddColumn<string>(
                name: "ImagePublicId",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePublicId",
                table: "Topics");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastCommentDate",
                table: "Topics",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);
        }
    }
}
