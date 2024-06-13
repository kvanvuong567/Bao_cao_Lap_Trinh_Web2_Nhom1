using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API_MPICTURE.Migrations
{
    /// <inheritdoc />
    public partial class ImageUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Image_Tags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Image_Tags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Image_Tags",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "UpDownImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpDownImages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpDownImages");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Thiên nhiên" },
                    { 2, "Du lịch" },
                    { 3, "Ẩm thực" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Núi" },
                    { 2, "Biển" },
                    { 3, "Thành phố" }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "CategoryId", "DateAdded", "Description", "Title", "URL" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 5, 26, 11, 37, 15, 0, DateTimeKind.Unspecified), "Hoàng hôn tuyệt đẹp trên bãi biển", "Hoàng hôn", "https://example.com/images/sunset.jpg" },
                    { 2, 1, new DateTime(2024, 5, 26, 11, 40, 22, 0, DateTimeKind.Unspecified), "Phong cảnh núi tráng lệ", "Núi", "https://example.com/images/mountains.jpg" },
                    { 3, 3, new DateTime(2024, 5, 26, 11, 42, 55, 0, DateTimeKind.Unspecified), "Khung cảnh rừng thanh bình", "Rừng", "https://example.com/images/forest.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Image_Tags",
                columns: new[] { "Id", "ImageId", "TagId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 3 },
                    { 3, 2, 1 }
                });
        }
    }
}
