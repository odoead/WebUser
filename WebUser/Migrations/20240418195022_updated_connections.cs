using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace WebUser.Migrations
{
    public partial class updated_connections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PromotionAttrValue");
            migrationBuilder.DropTable(name: "PromotionCategory");
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ActiveFrom = table.Column<DateTime>(nullable: false),
                    ActiveTo = table.Column<DateTime>(nullable: false),
                    DiscountVal = table.Column<double>(nullable: false),
                    DiscountPercent = table.Column<int>(nullable: false),
                    GetQuantity = table.Column<int>(nullable: false),
                    PointsValue = table.Column<int>(nullable: false),
                    PointsPercent = table.Column<int>(nullable: false),
                    PointsExpireDays = table.Column<int>(nullable: false),
                    MinPay = table.Column<double>(nullable: false),
                    BuyQuantity = table.Column<int>(nullable: false),
                    IsFirstOrder = table.Column<bool>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Promotions", x => x.ID)
            );
            migrationBuilder.CreateTable(
                name: "PromotionAttributeValues",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false),
                    AttributeValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionAttributeValues", x => new { x.PromotionId, x.AttributeValueId });
                    table.ForeignKey(
                        name: "FK_PromotionAttributeValues_AttributeValues_AttributeValueId",
                        column: x => x.AttributeValueId,
                        principalTable: "AttributeValues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PromotionAttributeValues_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateTable(
                name: "PromotionCategories",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCategories", x => new { x.PromotionId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_PromotionCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PromotionCategories_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateTable(
                name: "PromotionProducts",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionProducts", x => new { x.PromotionId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_PromotionProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PromotionProducts_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateTable(
                name: "PromotionPromProducts",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionPromProducts", x => new { x.PromotionId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_PromotionPromProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PromotionPromProducts_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateIndex(
                name: "IX_PromotionAttributeValues_AttributeValueId",
                table: "PromotionAttributeValues",
                column: "AttributeValueId"
            );
            migrationBuilder.CreateIndex(name: "IX_PromotionCategories_CategoryId", table: "PromotionCategories", column: "CategoryId");
            migrationBuilder.CreateIndex(name: "IX_PromotionProducts_ProductId", table: "PromotionProducts", column: "ProductId");
            migrationBuilder.CreateIndex(name: "IX_PromotionPromProducts_ProductId", table: "PromotionPromProducts", column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PromotionAttributeValues");
            migrationBuilder.DropTable(name: "PromotionCategories");
            migrationBuilder.DropTable(name: "PromotionProducts");
            migrationBuilder.DropTable(name: "PromotionPromProducts");
            migrationBuilder.DropPrimaryKey(name: "PK_Promotions", table: "Promotions");
            migrationBuilder.RenameTable(name: "Promotions", newName: "PromotionPromProduct");
            migrationBuilder.AddPrimaryKey(name: "PK_PromotionPromProduct", table: "PromotionPromProduct", column: "ID");
            migrationBuilder.CreateTable(
                name: "ProductPromotion",
                columns: table => new
                {
                    ProductsID = table.Column<int>(type: "int", nullable: false),
                    PromotionsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPromotion", x => new { x.ProductsID, x.PromotionsID });
                    table.ForeignKey(
                        name: "FK_ProductPromotion_Products_ProductsID",
                        column: x => x.ProductsID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ProductPromotion_PromotionPromProduct_PromotionsID",
                        column: x => x.PromotionsID,
                        principalTable: "PromotionPromProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateTable(
                name: "ProductPromotion1",
                columns: table => new
                {
                    PromoProductsID = table.Column<int>(type: "int", nullable: false),
                    PromoPromotionsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPromotion1", x => new { x.PromoProductsID, x.PromoPromotionsID });
                    table.ForeignKey(
                        name: "FK_ProductPromotion1_Products_PromoProductsID",
                        column: x => x.PromoProductsID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ProductPromotion1_PromotionPromProduct_PromoPromotionsID",
                        column: x => x.PromoPromotionsID,
                        principalTable: "PromotionPromProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateTable(
                name: "PromotionAttrValue",
                columns: table => new
                {
                    AttributeValuesID = table.Column<int>(type: "int", nullable: false),
                    PromotionsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionAttrValue", x => new { x.AttributeValuesID, x.PromotionsID });
                    table.ForeignKey(
                        name: "FK_PromotionAttrValue_AttributeValues_AttributeValuesID",
                        column: x => x.AttributeValuesID,
                        principalTable: "AttributeValues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PromotionAttrValue_PromotionPromProduct_PromotionsID",
                        column: x => x.PromotionsID,
                        principalTable: "PromotionPromProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateTable(
                name: "PromotionCategory",
                columns: table => new
                {
                    CategoriesID = table.Column<int>(type: "int", nullable: false),
                    PromotionsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCategory", x => new { x.CategoriesID, x.PromotionsID });
                    table.ForeignKey(
                        name: "FK_PromotionCategory_Categories_CategoriesID",
                        column: x => x.CategoriesID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PromotionCategory_PromotionPromProduct_PromotionsID",
                        column: x => x.PromotionsID,
                        principalTable: "PromotionPromProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateIndex(name: "IX_ProductPromotion_PromotionsID", table: "ProductPromotion", column: "PromotionsID");
            migrationBuilder.CreateIndex(name: "IX_ProductPromotion1_PromoPromotionsID", table: "ProductPromotion1", column: "PromoPromotionsID");
            migrationBuilder.CreateIndex(name: "IX_PromotionAttrValue_PromotionsID", table: "PromotionAttrValue", column: "PromotionsID");
            migrationBuilder.CreateIndex(name: "IX_PromotionCategory_PromotionsID", table: "PromotionCategory", column: "PromotionsID");
        }
    }
}
