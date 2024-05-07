namespace RemedioDiario.Data
{
    using Microsoft.EntityFrameworkCore;
    using RemedioDiario.Entitys;


    public class ApplicationDbContext : DbContext
    {
        public DbSet<RegistrarApp> RegistrarApp { get; set; }
        public DbSet<MedicamentosApp> MedicamentosApp { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistrarApp>().Property(e => e.Id);
            modelBuilder.Entity<MedicamentosApp>().HasKey(e => e.Id);
        }
    }
}
