using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace S_Buck_HW_4.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockCompanies",
                columns: table => new
                {
                    Symbol = table.Column<string>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCompanies", x => x.Symbol);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "UserStocks",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(nullable: false),
                    Shares = table.Column<int>(nullable: false),
                    Basis = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStocks", x => new { x.UserID, x.Symbol });
                    table.ForeignKey(
                        name: "FK_UserStocks_StockCompanies_Symbol",
                        column: x => x.Symbol,
                        principalTable: "StockCompanies",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserStocks_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserStockTrades",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(nullable: false),
                    TradeDateTime = table.Column<DateTime>(nullable: false),
                    Shares = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStockTrades", x => new { x.UserID, x.Symbol, x.TradeDateTime });
                    table.ForeignKey(
                        name: "FK_UserStockTrades_StockCompanies_Symbol",
                        column: x => x.Symbol,
                        principalTable: "StockCompanies",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserStockTrades_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserStockTrades_UserStocks_UserID_Symbol",
                        columns: x => new { x.UserID, x.Symbol },
                        principalTable: "UserStocks",
                        principalColumns: new[] { "UserID", "Symbol" },
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStocks_Symbol",
                table: "UserStocks",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockTrades_Symbol",
                table: "UserStockTrades",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStockTrades");

            migrationBuilder.DropTable(
                name: "UserStocks");

            migrationBuilder.DropTable(
                name: "StockCompanies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
