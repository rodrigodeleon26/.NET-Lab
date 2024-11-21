using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class paypaltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PagoPayPalId",
                table: "Facturas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PagosPayPal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    linkPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pagoId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosPayPal", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_PagoPayPalId",
                table: "Facturas",
                column: "PagoPayPalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_PagosPayPal_PagoPayPalId",
                table: "Facturas",
                column: "PagoPayPalId",
                principalTable: "PagosPayPal",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_PagosPayPal_PagoPayPalId",
                table: "Facturas");

            migrationBuilder.DropTable(
                name: "PagosPayPal");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_PagoPayPalId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "PagoPayPalId",
                table: "Facturas");
        }
    }
}
