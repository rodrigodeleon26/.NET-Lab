using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultaMedicaRecetasRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConsultaMedicaId",
                table: "Recetas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ConsultaMedicaId",
                table: "Recetas",
                column: "ConsultaMedicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recetas_ConsultasMedicas_ConsultaMedicaId",
                table: "Recetas",
                column: "ConsultaMedicaId",
                principalTable: "ConsultasMedicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recetas_ConsultasMedicas_ConsultaMedicaId",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_ConsultaMedicaId",
                table: "Recetas");

            migrationBuilder.DropColumn(
                name: "ConsultaMedicaId",
                table: "Recetas");
        }
    }
}
