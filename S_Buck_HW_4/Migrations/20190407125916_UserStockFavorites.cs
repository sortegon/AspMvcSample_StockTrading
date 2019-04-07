using Microsoft.EntityFrameworkCore.Migrations;

namespace S_Buck_HW_4.Migrations
{
    public partial class UserStockFavorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStockFavorites",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStockFavorites", x => new { x.UserID, x.Symbol });
                    table.ForeignKey(
                        name: "FK_UserStockFavorites_StockCompanies_Symbol",
                        column: x => x.Symbol,
                        principalTable: "StockCompanies",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStockFavorites_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStockFavorites_Symbol",
                table: "UserStockFavorites",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStockFavorites");
        }
    }
}
