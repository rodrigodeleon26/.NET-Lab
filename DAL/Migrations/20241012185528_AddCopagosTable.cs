using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCopagosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Copagos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticuloId = table.Column<long>(type: "bigint", nullable: false),
                    SeguroMedicoId = table.Column<long>(type: "bigint", nullable: false),
                    EspecialidadId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Copagos_Articulos_ArticuloId",
                        column: x => x.ArticuloId,
                        principalTable: "Articulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Copagos_Especialidades_EspecialidadId",
                        column: x => x.EspecialidadId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Copagos_SegurosMedicos_SeguroMedicoId",
                        column: x => x.SeguroMedicoId,
                        principalTable: "SegurosMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Copagos_ArticuloId",
                table: "Copagos",
                column: "ArticuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Copagos_EspecialidadId",
                table: "Copagos",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Copagos_SeguroMedicoId",
                table: "Copagos",
                column: "SeguroMedicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Copagos");
        }
    }
}
