using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace WebUser.Migrations
{
    public partial class edited_AttributeName_Category_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_AttributeNames_Categories_CategoryID", table: "AttributeNames");
            migrationBuilder.DropIndex(name: "IX_AttributeNames_CategoryID", table: "AttributeNames");
            migrationBuilder.DropColumn(name: "CategoryID", table: "AttributeNames");
            migrationBuilder.CreateTable(
                name: "AttributeNameCategories",
                columns: table => new
                {
                    AttributeNameID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeNameCategories", x => new { x.AttributeNameID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_AttributeNameCategories_AttributeNames_AttributeNameID",
                        column: x => x.AttributeNameID,
                        principalTable: "AttributeNames",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_AttributeNameCategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateIndex(name: "IX_AttributeNameCategories_CategoryID", table: "AttributeNameCategories", column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AttributeNameCategories");
            migrationBuilder.DropColumn(name: "DateCreated", table: "Products");
            migrationBuilder.AddColumn<int>(name: "CategoryID", table: "AttributeNames", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.CreateIndex(name: "IX_AttributeNames_CategoryID", table: "AttributeNames", column: "CategoryID");
            migrationBuilder.AddForeignKey(
                name: "FK_AttributeNames_Categories_CategoryID",
                table: "AttributeNames",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
