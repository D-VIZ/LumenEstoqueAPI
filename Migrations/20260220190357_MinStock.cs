using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LumenEstoque.Migrations
{
    /// <inheritdoc />
    public partial class MinStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinStock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinStock",
                table: "Products");
        }
    }
}
