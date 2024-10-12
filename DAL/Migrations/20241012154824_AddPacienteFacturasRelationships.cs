using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPacienteFacturasRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PacienteId",
                table: "Facturas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_PacienteId",
                table: "Facturas",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Pacientes_PacienteId",
                table: "Facturas",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Pacientes_PacienteId",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_PacienteId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Facturas");
        }
    }
}
