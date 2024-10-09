using Microsoft.EntityFrameworkCore;
using Shared;

namespace DAL
{
    public class DBContext : DbContext
    {
        private string _connectionString = "Server=sqlserver,1433;Database=HCE; User Id=sa; Password=Abc*123!;Encrypt=False;";

        public DBContext() { }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Paciente> Pacientes { get; set; }

        public static void UpdateDatabase()
        {
            using (var context = new DBContext())
            {
                context?.Database.Migrate();
            }
            Console.WriteLine("Base de datos actualizada.");
        }
    }
}