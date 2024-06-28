using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace WebUser.Migrations
{
    public partial class edited_ProductAttributeValue_connection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(name: "attID", table: "ProductAttributeValues");

        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.AddColumn<int>(name: "attID", table: "ProductAttributeValues", type: "int", nullable: false, defaultValue: 0);
    }
}
