using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadosCitasMedicasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosCitasMedicas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false, maxLength: 50)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCitasMedicas", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO EstadosCitasMedicas (Nombre) VALUES ('Cancelada'), ('Pendiente'), ('Completada'), ('NoAsistida'), ('Agendada');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadosCitasMedicas");

        }
    }
}
