using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atendimentos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MESAS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Numero = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Capacidade = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Localizacao = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                    QrCode = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "RAW(8)", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MESAS", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MESAS_Numero",
                table: "MESAS",
                column: "Numero",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MESAS");
        }
    }
}
