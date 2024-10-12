using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicosCalendariosEspecialidadesRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EspecialidadId",
                table: "Calendarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MedicoId",
                table: "Calendarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Calendarios_EspecialidadId",
                table: "Calendarios",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendarios_MedicoId",
                table: "Calendarios",
                column: "MedicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendarios_Especialidades_EspecialidadId",
                table: "Calendarios",
                column: "EspecialidadId",
                principalTable: "Especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendarios_Medicos_MedicoId",
                table: "Calendarios",
                column: "MedicoId",
                principalTable: "Medicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendarios_Especialidades_EspecialidadId",
                table: "Calendarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Calendarios_Medicos_MedicoId",
                table: "Calendarios");

            migrationBuilder.DropIndex(
                name: "IX_Calendarios_EspecialidadId",
                table: "Calendarios");

            migrationBuilder.DropIndex(
                name: "IX_Calendarios_MedicoId",
                table: "Calendarios");

            migrationBuilder.DropColumn(
                name: "EspecialidadId",
                table: "Calendarios");

            migrationBuilder.DropColumn(
                name: "MedicoId",
                table: "Calendarios");
        }
    }
}
