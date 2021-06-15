using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecordingStudio.Migrations
{
    public partial class EditOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Orders",
                newName: "ToDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDateTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StudioId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StudioId",
                table: "Orders",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Studios_StudioId",
                table: "Orders",
                column: "StudioId",
                principalTable: "Studios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Studios_StudioId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StudioId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FromDateTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ToDateTime",
                table: "Orders",
                newName: "DateTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
