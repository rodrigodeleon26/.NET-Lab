using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultorioCitasMedicasRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConsultorioId",
                table: "CitasMedicas",
                type: "bigint",
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CitasMedicas_ConsultorioId",
                table: "CitasMedicas",
                column: "ConsultorioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CitasMedicas_Consultorios_ConsultorioId",
                table: "CitasMedicas",
                column: "ConsultorioId",
                principalTable: "Consultorios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitasMedicas_Consultorios_ConsultorioId",
                table: "CitasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_CitasMedicas_ConsultorioId",
                table: "CitasMedicas");

            migrationBuilder.DropColumn(
                name: "ConsultorioId",
                table: "CitasMedicas");
        }
    }
}
