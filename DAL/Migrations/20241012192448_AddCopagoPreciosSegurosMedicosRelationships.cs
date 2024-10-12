using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCopagoPreciosSegurosMedicosRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Precios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CopagoId = table.Column<long>(type: "bigint", nullable: false),
                    SeguroMedicoId = table.Column<long>(type: "bigint", nullable: false),
                    PrecioBase = table.Column<float>(type: "real", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Precios_Copagos_CopagoId",
                        column: x => x.CopagoId,
                        principalTable: "Copagos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Precios_SegurosMedicos_SeguroMedicoId",
                        column: x => x.SeguroMedicoId,
                        principalTable: "SegurosMedicos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Precios_CopagoId",
                table: "Precios",
                column: "CopagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Precios_SeguroMedicoId",
                table: "Precios",
                column: "SeguroMedicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Precios");

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticuloId = table.Column<long>(type: "bigint", nullable: false),
                    EspecialidadId = table.Column<long>(type: "bigint", nullable: false),
                    SeguroMedicoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servicios_Articulos_ArticuloId",
                        column: x => x.ArticuloId,
                        principalTable: "Articulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servicios_Especialidades_EspecialidadId",
                        column: x => x.EspecialidadId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servicios_SegurosMedicos_SeguroMedicoId",
                        column: x => x.SeguroMedicoId,
                        principalTable: "SegurosMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_ArticuloId",
                table: "Servicios",
                column: "ArticuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_EspecialidadId",
                table: "Servicios",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_SeguroMedicoId",
                table: "Servicios",
                column: "SeguroMedicoId");
        }
    }
}
