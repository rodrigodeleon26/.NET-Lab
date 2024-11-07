using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicamentosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false, maxLength: 100),
                    Descripcion = table.Column<string>(nullable: true, maxLength: 500)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO Medicamentos (Nombre, Descripcion) VALUES 
                ('Loratadina', 'Antihistamínico para alergias'),
                ('Omeprazol', 'Inhibidor de la bomba de protones para reducir el ácido estomacal'),
                ('Metformina', 'Medicamento para la diabetes tipo 2'),
                ('Aspirina', 'Analgésico y antipirético'),
                ('Diclofenaco', 'Antiinflamatorio no esteroideo para dolores musculares y articulares'),
                ('Enalapril', 'Antihipertensivo para la presión arterial alta'),
                ('Clorfenamina', 'Antihistamínico para reacciones alérgicas');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicamentos");
        }
    }
}