using Microsoft.EntityFrameworkCore.Migrations;

namespace RecordingStudio.Migrations
{
    public partial class EditPriceLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Prices_PriceId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PriceId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Orders");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Studios",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Studios");

            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostPerHours = table.Column<double>(type: "float", nullable: false),
                    StudioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "Studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PriceId",
                table: "Orders",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_StudioId",
                table: "Prices",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Prices_PriceId",
                table: "Orders",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
