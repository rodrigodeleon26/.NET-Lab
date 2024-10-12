using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPacienteNotificacionesRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PacienteId",
                table: "Notificaciones",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_PacienteId",
                table: "Notificaciones",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Pacientes_PacienteId",
                table: "Notificaciones",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Pacientes_PacienteId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_PacienteId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Notificaciones");
        }
    }
}
