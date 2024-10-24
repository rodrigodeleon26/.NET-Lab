using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DAL
{
    public class DBContext : DbContext
    {
        private string _connectionString = "Server=sqlserver,1433;Database=HCE; User Id=sa; Password=Abc*123!;Encrypt=False;";
        //private string _connectionString = "Server=localhost,1433;Database=HCE; User Id=sa; Password=1234;Encrypt=False;";

        public DBContext() { }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, options =>
                options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CitasMedicas>()
               .HasOne(c => c.Paciente)
               .WithMany(p => p.CitasMedicas)
               .HasForeignKey(c => c.PacienteId)
               .IsRequired(false); // La relación con Paciente es opcional

            modelBuilder.Entity<CitasMedicas>()
                .HasOne(c => c.ConsultaMedica)
                .WithOne(cm => cm.CitaMedica)
                .HasForeignKey<ConsultasMedicas>(cm => cm.CitaMedicaId)
                .IsRequired(false); // La relación con ConsultasMedicas es opcional

            
            modelBuilder.Entity<Precios>()
                .HasOne(p => p.Copago)
                .WithMany(c => c.Precios)
                .HasForeignKey(p => p.CopagoId)
                .IsRequired(false); // La relación con Copago es opcional

            modelBuilder.Entity<Precios>()
                .HasOne(p => p.SeguroMedico)
                .WithMany(s => s.Precios)
                .HasForeignKey(p => p.SeguroMedicoId)
                .IsRequired(false); // La relación con SeguroMedico es opcional
        }


        //  Actualiza la base de datos
        public static void UpdateDatabase()
        {
            using (var context = new DBContext())
            {
                context?.Database.Migrate();
            }
        }

        //  Tablas
        public DbSet<Pacientes> Pacientes { get; set; }
        public DbSet<Medicos> Medicos { get; set; }
        public DbSet<Especialidades> Especialidades { get; set; }
        public DbSet<Calendarios> Calendarios { get; set; }
        public DbSet<CitasMedicas> CitasMedicas { get; set; }
        public DbSet<Consultorios> Consultorios { get; set; }
        public DbSet<ConsultasMedicas> ConsultasMedicas { get; set; }
        public DbSet<Recetas> Recetas { get; set; }
        public DbSet<Estudios> Estudios { get; set; }
        public DbSet<Notificaciones> Notificaciones { get; set; }
        public DbSet<Facturas> Facturas { get; set; }
        public DbSet<SegurosMedicos> SegurosMedicos { get; set; }
        public DbSet<Contratos> Contratos { get; set; }
        public DbSet<Articulos> Articulos { get; set; }
        public DbSet<Copagos> Copagos { get; set; }
        public DbSet<Precios> Precios { get; set; }
        public DbSet<EspecialidadesMedicos> EspecialidadesMedicos { get; set; }

    }
}
