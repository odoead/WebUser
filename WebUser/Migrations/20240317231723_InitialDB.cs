using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUser.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_categories_categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "categories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "promotions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountVal = table.Column<double>(type: "float", nullable: false),
                    DiscountPercent = table.Column<float>(type: "real", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_promotions_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountVal = table.Column<double>(type: "float", nullable: false),
                    DiscountPercent = table.Column<float>(type: "real", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_discounts_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_carts_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image", x => x.ID);
                    table.ForeignKey(
                        name: "FK_image_products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "products",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_image_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PointsUsed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_orders_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attributeNames",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    PromotionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attributeNames", x => x.ID);
                    table.ForeignKey(
                        name: "FK_attributeNames_categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "categories",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_attributeNames_promotions_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "promotions",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "cartItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_cartItems_carts_CartId",
                        column: x => x.CartId,
                        principalTable: "carts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coupons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountVal = table.Column<double>(type: "float", nullable: false),
                    DiscountPercent = table.Column<float>(type: "real", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons", x => x.ID);
                    table.ForeignKey(
                        name: "FK_coupons_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_coupons_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "orderProducts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderProducts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_orderProducts_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderProducts_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "points",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    isExpirable = table.Column<bool>(type: "bit", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_points", x => x.ID);
                    table.ForeignKey(
                        name: "FK_points_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_points_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attributeValues",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttributeNameID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attributeValues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_attributeValues_attributeNames_AttributeNameID",
                        column: x => x.AttributeNameID,
                        principalTable: "attributeNames",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_attributeValues_products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "products",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_attributeNames_CategoryID",
                table: "attributeNames",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_attributeNames_PromotionID",
                table: "attributeNames",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_attributeValues_AttributeNameID",
                table: "attributeValues",
                column: "AttributeNameID");

            migrationBuilder.CreateIndex(
                name: "IX_attributeValues_ProductID",
                table: "attributeValues",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_CartId",
                table: "cartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_carts_UserId",
                table: "carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_ParentCategoryId",
                table: "categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_OrderId",
                table: "coupons",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_UserId",
                table: "coupons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_discounts_ProductId",
                table: "discounts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_image_ProductID",
                table: "image",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_image_UserID",
                table: "image",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_orderProducts_OrderId",
                table: "orderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_orderProducts_ProductId",
                table: "orderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UserId",
                table: "orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_points_OrderId",
                table: "points",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_points_UserId",
                table: "points",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_promotions_CategoryId",
                table: "promotions",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attributeValues");

            migrationBuilder.DropTable(
                name: "cartItems");

            migrationBuilder.DropTable(
                name: "coupons");

            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "orderProducts");

            migrationBuilder.DropTable(
                name: "points");

            migrationBuilder.DropTable(
                name: "attributeNames");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "promotions");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
