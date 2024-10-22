using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCalendarioCitasMedicasRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CalendarioId",
                table: "CitasMedicas",
                type: "bigint",
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CitasMedicas_CalendarioId",
                table: "CitasMedicas",
                column: "CalendarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CitasMedicas_Calendarios_CalendarioId",
                table: "CitasMedicas",
                column: "CalendarioId",
                principalTable: "Calendarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitasMedicas_Calendarios_CalendarioId",
                table: "CitasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_CitasMedicas_CalendarioId",
                table: "CitasMedicas");

            migrationBuilder.DropColumn(
                name: "CalendarioId",
                table: "CitasMedicas");
        }
    }
}
