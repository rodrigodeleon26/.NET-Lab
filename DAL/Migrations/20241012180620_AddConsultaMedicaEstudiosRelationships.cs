using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultaMedicaEstudiosRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConsultaMedicaId",
                table: "Estudios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_ConsultaMedicaId",
                table: "Estudios",
                column: "ConsultaMedicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudios_ConsultasMedicas_ConsultaMedicaId",
                table: "Estudios",
                column: "ConsultaMedicaId",
                principalTable: "ConsultasMedicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudios_ConsultasMedicas_ConsultaMedicaId",
                table: "Estudios");

            migrationBuilder.DropIndex(
                name: "IX_Estudios_ConsultaMedicaId",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "ConsultaMedicaId",
                table: "Estudios");
        }
    }
}
