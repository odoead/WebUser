using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace WebUser.Migrations
{
    public partial class added_Productattributevalue_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_AttributeNames_Categories_CategoryId", table: "AttributeNames");
            migrationBuilder.DropForeignKey(name: "FK_AttributeValues_Products_ProductID", table: "AttributeValues");
            migrationBuilder.DropForeignKey(name: "FK_CartItems_Carts_CartId", table: "CartItems");
            migrationBuilder.DropForeignKey(name: "FK_CartItems_Products_ProductId", table: "CartItems");
            migrationBuilder.DropForeignKey(name: "FK_Carts_AspNetUsers_UserId", table: "Carts");
            migrationBuilder.DropForeignKey(name: "FK_Categories_Categories_ParentCategoryId", table: "Categories");
            migrationBuilder.DropForeignKey(name: "FK_Coupons_Orders_OrderId", table: "Coupons");
            migrationBuilder.DropForeignKey(name: "FK_Coupons_Products_ProductId", table: "Coupons");
            migrationBuilder.DropForeignKey(name: "FK_Discounts_Products_ProductId", table: "Discounts");
            migrationBuilder.DropForeignKey(name: "FK_OrderProducts_Orders_OrderId", table: "OrderProducts");
            migrationBuilder.DropForeignKey(name: "FK_OrderProducts_Products_ProductId", table: "OrderProducts");
            migrationBuilder.DropForeignKey(name: "FK_Orders_AspNetUsers_UserId", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Points_AspNetUsers_UserId", table: "Points");
            migrationBuilder.DropForeignKey(name: "FK_Points_Orders_OrderId", table: "Points");
            migrationBuilder.DropForeignKey(name: "FK_PromotionAttributeValues_AttributeValues_AttributeValueId", table: "PromotionAttributeValues");
            migrationBuilder.DropForeignKey(name: "FK_PromotionAttributeValues_Promotions_PromotionId", table: "PromotionAttributeValues");
            migrationBuilder.DropForeignKey(name: "FK_PromotionCategories_Categories_CategoryId", table: "PromotionCategories");
            migrationBuilder.DropForeignKey(name: "FK_PromotionCategories_Promotions_PromotionId", table: "PromotionCategories");
            migrationBuilder.DropForeignKey(name: "FK_PromotionProducts_Products_ProductId", table: "PromotionProducts");
            migrationBuilder.DropForeignKey(name: "FK_PromotionProducts_Promotions_PromotionId", table: "PromotionProducts");
            migrationBuilder.DropForeignKey(name: "FK_PromotionPromProducts_Products_ProductId", table: "PromotionPromProducts");
            migrationBuilder.DropForeignKey(name: "FK_PromotionPromProducts_Promotions_PromotionId", table: "PromotionPromProducts");
            migrationBuilder.DropIndex(name: "IX_AttributeValues_ProductID", table: "AttributeValues");
            migrationBuilder.DropColumn(name: "ProductID", table: "AttributeValues");
            migrationBuilder.RenameColumn(name: "ProductId", table: "PromotionPromProducts", newName: "ProductID");
            migrationBuilder.RenameColumn(name: "PromotionId", table: "PromotionPromProducts", newName: "PromotionID");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionPromProducts_ProductId",
                table: "PromotionPromProducts",
                newName: "IX_PromotionPromProducts_ProductID"
            );
            migrationBuilder.RenameColumn(name: "ProductId", table: "PromotionProducts", newName: "ProductID");
            migrationBuilder.RenameColumn(name: "PromotionId", table: "PromotionProducts", newName: "PromotionID");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionProducts_ProductId",
                table: "PromotionProducts",
                newName: "IX_PromotionProducts_ProductID"
            );
            migrationBuilder.RenameColumn(name: "CategoryId", table: "PromotionCategories", newName: "CategoryID");
            migrationBuilder.RenameColumn(name: "PromotionId", table: "PromotionCategories", newName: "PromotionID");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionCategories_CategoryId",
                table: "PromotionCategories",
                newName: "IX_PromotionCategories_CategoryID"
            );
            migrationBuilder.RenameColumn(name: "AttributeValueId", table: "PromotionAttributeValues", newName: "AttributeValueID");
            migrationBuilder.RenameColumn(name: "PromotionId", table: "PromotionAttributeValues", newName: "PromotionID");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionAttributeValues_AttributeValueId",
                table: "PromotionAttributeValues",
                newName: "IX_PromotionAttributeValues_AttributeValueID"
            );
            migrationBuilder.RenameColumn(name: "UserId", table: "Points", newName: "UserID");
            migrationBuilder.RenameColumn(name: "OrderId", table: "Points", newName: "OrderID");
            migrationBuilder.RenameIndex(name: "IX_Points_UserId", table: "Points", newName: "IX_Points_UserID");
            migrationBuilder.RenameIndex(name: "IX_Points_OrderId", table: "Points", newName: "IX_Points_OrderID");
            migrationBuilder.RenameColumn(name: "UserId", table: "Orders", newName: "UserID");
            migrationBuilder.RenameIndex(name: "IX_Orders_UserId", table: "Orders", newName: "IX_Orders_UserID");
            migrationBuilder.RenameColumn(name: "ProductId", table: "OrderProducts", newName: "ProductID");
            migrationBuilder.RenameColumn(name: "OrderId", table: "OrderProducts", newName: "OrderID");
            migrationBuilder.RenameIndex(name: "IX_OrderProducts_ProductId", table: "OrderProducts", newName: "IX_OrderProducts_ProductID");
            migrationBuilder.RenameIndex(name: "IX_OrderProducts_OrderId", table: "OrderProducts", newName: "IX_OrderProducts_OrderID");
            migrationBuilder.RenameColumn(name: "ProductId", table: "Discounts", newName: "ProductID");
            migrationBuilder.RenameIndex(name: "IX_Discounts_ProductId", table: "Discounts", newName: "IX_Discounts_ProductID");
            migrationBuilder.RenameColumn(name: "ProductId", table: "Coupons", newName: "ProductID");
            migrationBuilder.RenameColumn(name: "OrderId", table: "Coupons", newName: "OrderID");
            migrationBuilder.RenameIndex(name: "IX_Coupons_ProductId", table: "Coupons", newName: "IX_Coupons_ProductID");
            migrationBuilder.RenameIndex(name: "IX_Coupons_OrderId", table: "Coupons", newName: "IX_Coupons_OrderID");
            migrationBuilder.RenameColumn(name: "ParentCategoryId", table: "Categories", newName: "ParentCategoryID");
            migrationBuilder.RenameIndex(name: "IX_Categories_ParentCategoryId", table: "Categories", newName: "IX_Categories_ParentCategoryID");
            migrationBuilder.RenameColumn(name: "UserId", table: "Carts", newName: "UserID");
            migrationBuilder.RenameIndex(name: "IX_Carts_UserId", table: "Carts", newName: "IX_Carts_UserID");
            migrationBuilder.RenameColumn(name: "ProductId", table: "CartItems", newName: "ProductID");
            migrationBuilder.RenameColumn(name: "CartId", table: "CartItems", newName: "CartID");
            migrationBuilder.RenameIndex(name: "IX_CartItems_ProductId", table: "CartItems", newName: "IX_CartItems_ProductID");
            migrationBuilder.RenameIndex(name: "IX_CartItems_CartId", table: "CartItems", newName: "IX_CartItems_CartID");
            migrationBuilder.RenameColumn(name: "CategoryId", table: "AttributeNames", newName: "CategoryID");
            migrationBuilder.RenameIndex(name: "IX_AttributeNames_CategoryId", table: "AttributeNames", newName: "IX_AttributeNames_CategoryID");
            migrationBuilder.CreateTable(
                name: "ProductAttributeValues",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    AttributeValueID = table.Column<int>(type: "int", nullable: false),
                    attID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValues", x => new { x.AttributeValueID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_AttributeValues_AttributeValueID",
                        column: x => x.AttributeValueID,
                        principalTable: "AttributeValues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
            migrationBuilder.CreateIndex(name: "IX_ProductAttributeValues_ProductID", table: "ProductAttributeValues", column: "ProductID");
            migrationBuilder.AddForeignKey(
                name: "FK_AttributeNames_Categories_CategoryID",
                table: "AttributeNames",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartID",
                table: "CartItems",
                column: "CartID",
                principalTable: "Carts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductID",
                table: "CartItems",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_UserID",
                table: "Carts",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryID",
                table: "Categories",
                column: "ParentCategoryID",
                principalTable: "Categories",
                principalColumn: "ID"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Orders_OrderID",
                table: "Coupons",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Products_ProductID",
                table: "Coupons",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Products_ProductID",
                table: "Discounts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderID",
                table: "OrderProducts",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductID",
                table: "OrderProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserID",
                table: "Orders",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Points_AspNetUsers_UserID",
                table: "Points",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Points_Orders_OrderID",
                table: "Points",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionAttributeValues_AttributeValues_AttributeValueID",
                table: "PromotionAttributeValues",
                column: "AttributeValueID",
                principalTable: "AttributeValues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionAttributeValues_Promotions_PromotionID",
                table: "PromotionAttributeValues",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionCategories_Categories_CategoryID",
                table: "PromotionCategories",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionCategories_Promotions_PromotionID",
                table: "PromotionCategories",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProducts_Products_ProductID",
                table: "PromotionProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProducts_Promotions_PromotionID",
                table: "PromotionProducts",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionPromProducts_Products_ProductID",
                table: "PromotionPromProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionPromProducts_Promotions_PromotionID",
                table: "PromotionPromProducts",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_AttributeNames_Categories_CategoryID", table: "AttributeNames");
            migrationBuilder.DropForeignKey(name: "FK_CartItems_Carts_CartID", table: "CartItems");
            migrationBuilder.DropForeignKey(name: "FK_CartItems_Products_ProductID", table: "CartItems");
            migrationBuilder.DropForeignKey(name: "FK_Carts_AspNetUsers_UserID", table: "Carts");
            migrationBuilder.DropForeignKey(name: "FK_Categories_Categories_ParentCategoryID", table: "Categories");
            migrationBuilder.DropForeignKey(name: "FK_Coupons_Orders_OrderID", table: "Coupons");
            migrationBuilder.DropForeignKey(name: "FK_Coupons_Products_ProductID", table: "Coupons");
            migrationBuilder.DropForeignKey(name: "FK_Discounts_Products_ProductID", table: "Discounts");
            migrationBuilder.DropForeignKey(name: "FK_OrderProducts_Orders_OrderID", table: "OrderProducts");
            migrationBuilder.DropForeignKey(name: "FK_OrderProducts_Products_ProductID", table: "OrderProducts");
            migrationBuilder.DropForeignKey(name: "FK_Orders_AspNetUsers_UserID", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Points_AspNetUsers_UserID", table: "Points");
            migrationBuilder.DropForeignKey(name: "FK_Points_Orders_OrderID", table: "Points");
            migrationBuilder.DropForeignKey(name: "FK_PromotionAttributeValues_AttributeValues_AttributeValueID", table: "PromotionAttributeValues");
            migrationBuilder.DropForeignKey(name: "FK_PromotionAttributeValues_Promotions_PromotionID", table: "PromotionAttributeValues");
            migrationBuilder.DropForeignKey(name: "FK_PromotionCategories_Categories_CategoryID", table: "PromotionCategories");
            migrationBuilder.DropForeignKey(name: "FK_PromotionCategories_Promotions_PromotionID", table: "PromotionCategories");
            migrationBuilder.DropForeignKey(name: "FK_PromotionProducts_Products_ProductID", table: "PromotionProducts");
            migrationBuilder.DropForeignKey(name: "FK_PromotionProducts_Promotions_PromotionID", table: "PromotionProducts");
            migrationBuilder.DropForeignKey(name: "FK_PromotionPromProducts_Products_ProductID", table: "PromotionPromProducts");
            migrationBuilder.DropForeignKey(name: "FK_PromotionPromProducts_Promotions_PromotionID", table: "PromotionPromProducts");
            migrationBuilder.DropTable(name: "ProductAttributeValues");
            migrationBuilder.RenameColumn(name: "ProductID", table: "PromotionPromProducts", newName: "ProductId");
            migrationBuilder.RenameColumn(name: "PromotionID", table: "PromotionPromProducts", newName: "PromotionId");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionPromProducts_ProductID",
                table: "PromotionPromProducts",
                newName: "IX_PromotionPromProducts_ProductId"
            );
            migrationBuilder.RenameColumn(name: "ProductID", table: "PromotionProducts", newName: "ProductId");
            migrationBuilder.RenameColumn(name: "PromotionID", table: "PromotionProducts", newName: "PromotionId");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionProducts_ProductID",
                table: "PromotionProducts",
                newName: "IX_PromotionProducts_ProductId"
            );
            migrationBuilder.RenameColumn(name: "CategoryID", table: "PromotionCategories", newName: "CategoryId");
            migrationBuilder.RenameColumn(name: "PromotionID", table: "PromotionCategories", newName: "PromotionId");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionCategories_CategoryID",
                table: "PromotionCategories",
                newName: "IX_PromotionCategories_CategoryId"
            );
            migrationBuilder.RenameColumn(name: "AttributeValueID", table: "PromotionAttributeValues", newName: "AttributeValueId");
            migrationBuilder.RenameColumn(name: "PromotionID", table: "PromotionAttributeValues", newName: "PromotionId");
            migrationBuilder.RenameIndex(
                name: "IX_PromotionAttributeValues_AttributeValueID",
                table: "PromotionAttributeValues",
                newName: "IX_PromotionAttributeValues_AttributeValueId"
            );
            migrationBuilder.RenameColumn(name: "UserID", table: "Points", newName: "UserId");
            migrationBuilder.RenameColumn(name: "OrderID", table: "Points", newName: "OrderId");
            migrationBuilder.RenameIndex(name: "IX_Points_UserID", table: "Points", newName: "IX_Points_UserId");
            migrationBuilder.RenameIndex(name: "IX_Points_OrderID", table: "Points", newName: "IX_Points_OrderId");
            migrationBuilder.RenameColumn(name: "UserID", table: "Orders", newName: "UserId");
            migrationBuilder.RenameIndex(name: "IX_Orders_UserID", table: "Orders", newName: "IX_Orders_UserId");
            migrationBuilder.RenameColumn(name: "ProductID", table: "OrderProducts", newName: "ProductId");
            migrationBuilder.RenameColumn(name: "OrderID", table: "OrderProducts", newName: "OrderId");
            migrationBuilder.RenameIndex(name: "IX_OrderProducts_ProductID", table: "OrderProducts", newName: "IX_OrderProducts_ProductId");
            migrationBuilder.RenameIndex(name: "IX_OrderProducts_OrderID", table: "OrderProducts", newName: "IX_OrderProducts_OrderId");
            migrationBuilder.RenameColumn(name: "ProductID", table: "Discounts", newName: "ProductId");
            migrationBuilder.RenameIndex(name: "IX_Discounts_ProductID", table: "Discounts", newName: "IX_Discounts_ProductId");
            migrationBuilder.RenameColumn(name: "ProductID", table: "Coupons", newName: "ProductId");
            migrationBuilder.RenameColumn(name: "OrderID", table: "Coupons", newName: "OrderId");
            migrationBuilder.RenameIndex(name: "IX_Coupons_ProductID", table: "Coupons", newName: "IX_Coupons_ProductId");
            migrationBuilder.RenameIndex(name: "IX_Coupons_OrderID", table: "Coupons", newName: "IX_Coupons_OrderId");
            migrationBuilder.RenameColumn(name: "ParentCategoryID", table: "Categories", newName: "ParentCategoryId");
            migrationBuilder.RenameIndex(name: "IX_Categories_ParentCategoryID", table: "Categories", newName: "IX_Categories_ParentCategoryId");
            migrationBuilder.RenameColumn(name: "UserID", table: "Carts", newName: "UserId");
            migrationBuilder.RenameIndex(name: "IX_Carts_UserID", table: "Carts", newName: "IX_Carts_UserId");
            migrationBuilder.RenameColumn(name: "ProductID", table: "CartItems", newName: "ProductId");
            migrationBuilder.RenameColumn(name: "CartID", table: "CartItems", newName: "CartId");
            migrationBuilder.RenameIndex(name: "IX_CartItems_ProductID", table: "CartItems", newName: "IX_CartItems_ProductId");
            migrationBuilder.RenameIndex(name: "IX_CartItems_CartID", table: "CartItems", newName: "IX_CartItems_CartId");
            migrationBuilder.RenameColumn(name: "CategoryID", table: "AttributeNames", newName: "CategoryId");
            migrationBuilder.RenameIndex(name: "IX_AttributeNames_CategoryID", table: "AttributeNames", newName: "IX_AttributeNames_CategoryId");
            migrationBuilder.AddColumn<int>(name: "ProductID", table: "AttributeValues", type: "int", nullable: true);
            migrationBuilder.CreateIndex(name: "IX_AttributeValues_ProductID", table: "AttributeValues", column: "ProductID");
            migrationBuilder.AddForeignKey(
                name: "FK_AttributeNames_Categories_CategoryId",
                table: "AttributeNames",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValues_Products_ProductID",
                table: "AttributeValues",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "ID"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Orders_OrderId",
                table: "Coupons",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "ID"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Products_ProductId",
                table: "Coupons",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Products_ProductId",
                table: "Discounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Points_AspNetUsers_UserId",
                table: "Points",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_Points_Orders_OrderId",
                table: "Points",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "ID"
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionAttributeValues_AttributeValues_AttributeValueId",
                table: "PromotionAttributeValues",
                column: "AttributeValueId",
                principalTable: "AttributeValues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionAttributeValues_Promotions_PromotionId",
                table: "PromotionAttributeValues",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionCategories_Categories_CategoryId",
                table: "PromotionCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionCategories_Promotions_PromotionId",
                table: "PromotionCategories",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProducts_Products_ProductId",
                table: "PromotionProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProducts_Promotions_PromotionId",
                table: "PromotionProducts",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionPromProducts_Products_ProductId",
                table: "PromotionPromProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                name: "FK_PromotionPromProducts_Promotions_PromotionId",
                table: "PromotionPromProducts",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
