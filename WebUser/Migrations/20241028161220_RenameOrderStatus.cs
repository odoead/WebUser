using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUser.Migrations
{
    public partial class RenameOrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "IsCompleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "Orders",
                newName: "Status");
        }
    }
}
