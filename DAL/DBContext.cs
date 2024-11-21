using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Shared;

namespace DAL
{
    public class DBContext : IdentityDbContext<AppUsers>
    {
        private string _connectionString = GlobalFunctions.GetConnectionString();
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

            //modelBuilder.Entity<CitasMedicas>()
            //   .HasOne(c => c.Paciente)
            //   .WithMany(p => p.CitasMedicas)
            //   .HasForeignKey(c => c.PacienteId)
            //   .IsRequired(false); // La relación con Paciente es opcional

            modelBuilder.Entity<CitasMedicas>()
                .HasOne(c => c.ConsultaMedica) // CitasMedicas tiene una referencia a ConsultasMedicas
                .WithMany() // No hay referencia inversa desde ConsultasMedicas
                .HasForeignKey(c => c.ConsultaMedicaId) // La clave foránea está en CitasMedicas
                .IsRequired(false); // Relación opcional



            modelBuilder.Entity<Precios>()
                .HasOne(p => p.Copago)
                .WithMany(c => c.Precios)
                .HasForeignKey(p => p.CopagoId)
                .OnDelete(DeleteBehavior.Cascade) // Configurar eliminación en cascada
                .IsRequired(false); // La relación con Copago es opcional

            modelBuilder.Entity<Precios>()
                .HasOne(p => p.SeguroMedico)
                .WithMany(s => s.Precios)
                .HasForeignKey(p => p.SeguroMedicoId)
                .OnDelete(DeleteBehavior.Cascade) // Configurar eliminación en cascada
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
        public DbSet<Medicamentos> Medicamentos { get; set; }
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
        public DbSet<PagosPayPal> PagosPayPal { get; set; }
    }
}
