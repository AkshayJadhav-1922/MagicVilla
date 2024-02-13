using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddedFKConstraintToVillaNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillId",
                table: "VillaNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 19, 59, 12, 180, DateTimeKind.Local).AddTicks(5716));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 19, 59, 12, 180, DateTimeKind.Local).AddTicks(5730));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 19, 59, 12, 180, DateTimeKind.Local).AddTicks(5733));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 19, 59, 12, 180, DateTimeKind.Local).AddTicks(5736));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 19, 59, 12, 180, DateTimeKind.Local).AddTicks(5738));

            migrationBuilder.CreateIndex(
                name: "IX_VillaNumbers_VillId",
                table: "VillaNumbers",
                column: "VillId");

            migrationBuilder.AddForeignKey(
                name: "FK_VillaNumbers_Villas_VillId",
                table: "VillaNumbers",
                column: "VillId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillaNumbers_Villas_VillId",
                table: "VillaNumbers");

            migrationBuilder.DropIndex(
                name: "IX_VillaNumbers_VillId",
                table: "VillaNumbers");

            migrationBuilder.DropColumn(
                name: "VillId",
                table: "VillaNumbers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 18, 59, 26, 144, DateTimeKind.Local).AddTicks(6522));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 18, 59, 26, 144, DateTimeKind.Local).AddTicks(6548));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 18, 59, 26, 144, DateTimeKind.Local).AddTicks(6550));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 18, 59, 26, 144, DateTimeKind.Local).AddTicks(6552));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 12, 18, 59, 26, 144, DateTimeKind.Local).AddTicks(6554));
        }
    }
}
