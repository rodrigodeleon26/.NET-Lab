using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MigracionClaveForanea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitasMedicas_Consultorios_ConsultorioId",
                table: "CitasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_ConsultaMedicaId",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_CitasMedicas_ConsultorioId",
                table: "CitasMedicas");

            migrationBuilder.DropColumn(
                name: "ConsultorioId",
                table: "CitasMedicas");

            migrationBuilder.RenameColumn(
                name: "DiasSemana",
                table: "Calendarios",
                newName: "DiasSemanaString");

            migrationBuilder.AddColumn<long>(
                name: "ConsultoriosId",
                table: "CitasMedicas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConsultorioId",
                table: "Calendarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ConsultaMedicaId",
                table: "Recetas",
                column: "ConsultaMedicaId");

            migrationBuilder.CreateIndex(
                name: "IX_CitasMedicas_ConsultoriosId",
                table: "CitasMedicas",
                column: "ConsultoriosId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendarios_ConsultorioId",
                table: "Calendarios",
                column: "ConsultorioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendarios_Consultorios_ConsultorioId",
                table: "Calendarios",
                column: "ConsultorioId",
                principalTable: "Consultorios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CitasMedicas_Consultorios_ConsultoriosId",
                table: "CitasMedicas",
                column: "ConsultoriosId",
                principalTable: "Consultorios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendarios_Consultorios_ConsultorioId",
                table: "Calendarios");

            migrationBuilder.DropForeignKey(
                name: "FK_CitasMedicas_Consultorios_ConsultoriosId",
                table: "CitasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_Recetas_ConsultaMedicaId",
                table: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_CitasMedicas_ConsultoriosId",
                table: "CitasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_Calendarios_ConsultorioId",
                table: "Calendarios");

            migrationBuilder.DropColumn(
                name: "ConsultoriosId",
                table: "CitasMedicas");

            migrationBuilder.DropColumn(
                name: "ConsultorioId",
                table: "Calendarios");

            migrationBuilder.RenameColumn(
                name: "DiasSemanaString",
                table: "Calendarios",
                newName: "DiasSemana");

            migrationBuilder.AddColumn<long>(
                name: "ConsultorioId",
                table: "CitasMedicas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ConsultaMedicaId",
                table: "Recetas",
                column: "ConsultaMedicaId",
                unique: true);

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
    }
}
