using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPacienteContratoSegurosMedicosRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PacienteId",
                table: "Contratos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SeguroMedicoId",
                table: "Contratos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_PacienteId",
                table: "Contratos",
                column: "PacienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_SeguroMedicoId",
                table: "Contratos",
                column: "SeguroMedicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_Pacientes_PacienteId",
                table: "Contratos",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_SegurosMedicos_SeguroMedicoId",
                table: "Contratos",
                column: "SeguroMedicoId",
                principalTable: "SegurosMedicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_Pacientes_PacienteId",
                table: "Contratos");

            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_SegurosMedicos_SeguroMedicoId",
                table: "Contratos");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_PacienteId",
                table: "Contratos");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_SeguroMedicoId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "SeguroMedicoId",
                table: "Contratos");
        }
    }
}
