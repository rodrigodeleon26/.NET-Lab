using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTiposEstudiosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposEstudios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false, maxLength: 50)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposEstudios", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO TiposEstudios (Nombre) VALUES ('Radiografía'), ('Tomografía'), ('Resonancia Magnética'), ('Ecografía'), ('Análisis de Sangre'), ('Análisis de Orina'), ('Análisis de Heces'), ('Electrocardiograma'), ('Espirometría'), ('Colonoscopia'), ('Endoscopia'), ('Mamografía'), ('Densitometría Ósea'), ('Prueba de Esfuerzo'), ('Holter'), ('Ecocardiograma');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
