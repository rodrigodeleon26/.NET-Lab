using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCampoActivoCalendario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Precios_Copagos_CopagoId",
            //    table: "Precios");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Precios_SegurosMedicos_SeguroMedicoId",
            //    table: "Precios");

            //migrationBuilder.AlterColumn<long>(
            //    name: "SeguroMedicoId",
            //    table: "Precios",
            //    type: "bigint",
            //    nullable: true,
            //    oldClrType: typeof(long),
            //    oldType: "bigint");

            //migrationBuilder.AlterColumn<long>(
            //    name: "CopagoId",
            //    table: "Precios",
            //    type: "bigint",
            //    nullable: true,
            //    oldClrType: typeof(long),
            //    oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Calendarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.CreateTable(
            //    name: "Medicamentos",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Medicamentos", x => x.Id);
            //    });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Precios_Copagos_CopagoId",
            //    table: "Precios",
            //    column: "CopagoId",
            //    principalTable: "Copagos",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Precios_SegurosMedicos_SeguroMedicoId",
            //    table: "Precios",
            //    column: "SeguroMedicoId",
            //    principalTable: "SegurosMedicos",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Precios_Copagos_CopagoId",
                table: "Precios");

            migrationBuilder.DropForeignKey(
                name: "FK_Precios_SegurosMedicos_SeguroMedicoId",
                table: "Precios");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Calendarios");

            migrationBuilder.AlterColumn<long>(
                name: "SeguroMedicoId",
                table: "Precios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CopagoId",
                table: "Precios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Precios_Copagos_CopagoId",
                table: "Precios",
                column: "CopagoId",
                principalTable: "Copagos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Precios_SegurosMedicos_SeguroMedicoId",
                table: "Precios",
                column: "SeguroMedicoId",
                principalTable: "SegurosMedicos",
                principalColumn: "Id");
        }
    }
}
