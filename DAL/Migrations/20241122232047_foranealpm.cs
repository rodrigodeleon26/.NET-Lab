using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class foranealpm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CitasMedicas_CopagoId",
                table: "CitasMedicas",
                column: "CopagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CitasMedicas_Copagos_CopagoId",
                table: "CitasMedicas",
                column: "CopagoId",
                principalTable: "Copagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitasMedicas_Copagos_CopagoId",
                table: "CitasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_CitasMedicas_CopagoId",
                table: "CitasMedicas");
        }
    }
}
