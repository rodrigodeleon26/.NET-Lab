using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCitaMedicaConsultaMedicaRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CitaMedicaId",
                table: "ConsultasMedicas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultasMedicas_CitaMedicaId",
                table: "ConsultasMedicas",
                column: "CitaMedicaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultasMedicas_CitasMedicas_CitaMedicaId",
                table: "ConsultasMedicas",
                column: "CitaMedicaId",
                principalTable: "CitasMedicas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultasMedicas_CitasMedicas_CitaMedicaId",
                table: "ConsultasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_ConsultasMedicas_CitaMedicaId",
                table: "ConsultasMedicas");

            migrationBuilder.DropColumn(
                name: "CitaMedicaId",
                table: "ConsultasMedicas");
        }
    }
}
