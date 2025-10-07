using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atendimentos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComandaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "MESAS",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "MESAS",
                type: "RAW(8)",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "RAW(2000)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MESAS",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataContratacao",
                table: "GARCONS",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.CreateTable(
                name: "COMANDAS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    MesaId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    GarcomId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ClienteId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataHoraAbertura = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    DataHoraFechamento = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COMANDAS_GARCONS_GarcomId",
                        column: x => x.GarcomId,
                        principalTable: "GARCONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COMANDAS_MESAS_MesaId",
                        column: x => x.MesaId,
                        principalTable: "MESAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMANDAS_GarcomId",
                table: "COMANDAS",
                column: "GarcomId");

            migrationBuilder.CreateIndex(
                name: "IX_COMANDAS_MesaId",
                table: "COMANDAS",
                column: "MesaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMANDAS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "MESAS",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "MESAS",
                type: "RAW(2000)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "RAW(8)",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MESAS",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataContratacao",
                table: "GARCONS",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");
        }
    }
}
