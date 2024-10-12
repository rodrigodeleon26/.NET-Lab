using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPacienteCitasMedicasRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PacienteId",
                table: "CitasMedicas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CitasMedicas_PacienteId",
                table: "CitasMedicas",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CitasMedicas_Pacientes_PacienteId",
                table: "CitasMedicas",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitasMedicas_Pacientes_PacienteId",
                table: "CitasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_CitasMedicas_PacienteId",
                table: "CitasMedicas");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "CitasMedicas");
        }
    }
}
